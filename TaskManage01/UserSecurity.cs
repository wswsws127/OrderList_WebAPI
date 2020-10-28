using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskDataAccess;

namespace TaskManage01
{
    public class UserSecurity
    {
        public static bool Login(string username, string password)
        {
            using (TaskDBEntities entities = new TaskDBEntities())
            {
                return entities.tblUser.Any(user =>
                       user.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
                                          && user.Password == password);
            }
        }
    }
}