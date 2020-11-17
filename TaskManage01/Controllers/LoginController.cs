using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using TaskManage01.Models;
using TaskDataAccess;
using System.Linq;

namespace TaskManage01.Controllers
{
    public class LoginController : ApiController
    {
        public tblUser tblUserValid(string userName, string password)
        {
            using (TaskDBEntities entities = new TaskDBEntities())
            {
                //Go to https://localhost:44350/api/users, post 请求并附加json格式的username和password。
                //去tblUser里寻找是否有这个user
                var user = entities.tblUser.FirstOrDefault(e => e.Username == userName && e.Password == password);
                //bool ifValid = user!=null;

                return user;
            }
        }

        
        [HttpPost]
        public HttpResponseMessage Login([FromBody] string userName, string password)
        {
            using (TaskDBEntities entities = new TaskDBEntities())
            {
                try
                {
                    tblUser user = tblUserValid(userName, password);
                    if (user == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid User", Configuration.Formatters.JsonFormatter);
                    }
                    else
                    {

                        //AuthenticationModule authentication = new AuthenticationModule();
                        //string token = authentication.GenerateTokenForUser(user.Username, user.UserID);
                        return Request.CreateResponse(HttpStatusCode.OK, Configuration.Formatters.JsonFormatter);
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }

        [HttpPost]
        //接收HTTP请求，验证用户名和密码。如果user valid，我们将token发回。
        public IHttpActionResult Authenticate([FromBody] LoginRequest loginUser)
        {
            using (TaskDBEntities entities = new TaskDBEntities())
            {
                var loginResponse = new LoginResponse { };
                LoginRequest loginrequest = new LoginRequest { };
                loginrequest.Username = loginUser.Username.ToLower();
                loginrequest.Password = loginUser.Password;

                IHttpActionResult response;
                HttpResponseMessage responseMsg = new HttpResponseMessage();
                bool isUsernamePasswordValid = false;

                //根据用户名、密码，到DB里查找
                var user = entities.tblUser.FirstOrDefault(e => e.Username == loginrequest.Username && e.Password == loginrequest.Password);

                if (loginUser != null)
                    isUsernamePasswordValid = user != null ? true : false;
                // if credentials are valid
                if (isUsernamePasswordValid)
                {
                    string token = createToken(loginrequest.Username);
                    //return the token
                    return Ok<string>(token);
                    //return Request.CreateResponse(HttpStatusCode.OK, Configuration.Formatters.JsonFormatter);
                }
                else
                {
                    // if credentials are not valid send unauthorized status code in response
                    loginResponse.responseMsg.StatusCode = HttpStatusCode.Unauthorized;
                    response = ResponseMessage(loginResponse.responseMsg);
                    return response;
                    //return Request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid User", Configuration.Formatters.JsonFormatter);
                }
            }
        }

        private string createToken(string username)
        {
            //Set issued at date
            DateTime issuedAt = DateTime.UtcNow;
            //set the time when it expires
            DateTime expires = DateTime.UtcNow.AddDays(7);

            //http://stackoverflow.com/questions/18223868/how-to-encrypt-jwt-security-token
            var tokenHandler = new JwtSecurityTokenHandler();

            //create a identity and add claims to the user which we want to log in
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username)
            });

            const string sec = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
            var now = DateTime.UtcNow;
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);


            //create the jwt
            var token =
                (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(issuer: "http://localhost:44350", audience: "http://localhost:44350",
                        subject: claimsIdentity, notBefore: issuedAt, expires: expires, signingCredentials: signingCredentials);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}