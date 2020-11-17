using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using TaskDataAccess;

namespace TaskManage01.Controllers
{
    [RoutePrefix("api/Users")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsersController : ApiController
    {


        //private TaskDBEntities db = new TaskDBEntities();
        
        // GET: api/Users
        [HttpGet]
        public IEnumerable<tblUser> Get()
        {
            using (TaskDBEntities entities = new TaskDBEntities())
            {
                return entities.tblUser.ToList();
            }
        }

       
        public tblUser tblUserValid(string userName, string password)
        {
            using (TaskDBEntities entities = new TaskDBEntities())
            {
                //Go to https://localhost:44350/api/users/tblUserValid, post 请求并附加json格式的username和password。
                //去tblUser里寻找是否有这个user
                var user = entities.tblUser.FirstOrDefault(e => e.Username == userName && e.Password==password);
                //bool ifValid = user!=null;
               
                return user;
            }
        }

        [Route("Login")]
        [HttpPost]
        public HttpResponseMessage Login(string userName, string password)
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
                catch(Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }

        // GET: api/Users/5
        //[ResponseType(typeof(tblUser))]
        //public IHttpActionResult GettblUser(int id)
        //{
        //    tblUser tblUser = db.tblUser.Find(id);
        //    if (tblUser == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(tblUser);
        //}

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PuttblUser(int id, tblUser tblUser)
        {
            using (TaskDBEntities entities = new TaskDBEntities())
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != tblUser.UserID)
                {
                    return BadRequest();
                }

                entities.Entry(tblUser).State = EntityState.Modified;

                try
                {
                    entities.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tblUserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return StatusCode(HttpStatusCode.NoContent);
            }
        }

        // POST: api/Users
        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        //[Route("Register")]
        
        public HttpResponseMessage Register([FromBody] tblUser newUser)
        {
            try
            {
                using (TaskDBEntities entities = new TaskDBEntities())
                {

                    entities.tblUser.Add(newUser);
                    entities.SaveChanges();

                    //show http status code response: 201 created 
                    var message = Request.CreateResponse(HttpStatusCode.Created, newUser);
                    //create an instance of Uri of the newly created item
                    message.Headers.Location = new Uri(Request.RequestUri + newUser.UserID.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            using (TaskDBEntities entities = new TaskDBEntities())
            {
                if (disposing)
                {
                    entities.Dispose();
                }
                base.Dispose(disposing);
            }
        }

        private bool tblUserExists(int id)
        {
            using (TaskDBEntities entities = new TaskDBEntities())
            {
                return entities.tblUser.Count(e => e.UserID == id) > 0;
            }
        }
    }
}