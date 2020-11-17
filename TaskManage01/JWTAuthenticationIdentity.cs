using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace TaskManage01
{
    public class JWTAuthenticationIdentity : GenericIdentity
    {

        public string UserName { get; set; }
        public int UserId { get; set; }

        public JWTAuthenticationIdentity(string userName)
            : base(userName)
        {
            UserName = userName;
        }


    }
}