using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TaskDataAccess;

namespace TaskManage01.Controllers
{
    public class UsersController : ApiController
    {
        //private TaskDBEntities db = new TaskDBEntities();

        // GET: api/Users
        public IEnumerable<tblUser> Get()
        {
            using (TaskDBEntities entities = new TaskDBEntities())
            {
                return entities.tblUser.ToList();
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
        public HttpResponseMessage Post([FromBody] tblUser newUser)
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