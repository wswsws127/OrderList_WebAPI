using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TaskDataAccess;

namespace TaskManage01.Controllers
{
    public class TasksController : ApiController
    {
        public IEnumerable<tblTask> Get()
        {
            using (TaskDBEntities entities = new TaskDBEntities()) 
            {
                return entities.tblTask.ToList();
            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (TaskDBEntities entities = new TaskDBEntities())
            {
                var entity= entities.tblTask.FirstOrDefault(t => t.QuoteID==id);
                if (entity != null)
                {
                    //return 200 OK
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else 
                {
                    //return 404 not found
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Task with ID="+id.ToString() +" is not found.");
                }
            }
        }

        public HttpResponseMessage Post([FromBody]tblTask newTask) 
        {
            try {
                using (TaskDBEntities entities = new TaskDBEntities())
                {
                    entities.tblTask.Add(newTask);
                    entities.SaveChanges();

                    //show http status code response: 201 created 
                    var message = Request.CreateResponse(HttpStatusCode.Created, newTask);
                    //create an instance of Uri of the newly created item
                    message.Headers.Location = new Uri(Request.RequestUri + newTask.QuoteID.ToString());
                    return message;
                }
            }
            catch (Exception ex) 
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Delete(int id) 
        {
            try
            {
                using (TaskDBEntities entities = new TaskDBEntities())
                {
                    var entity = entities.tblTask.FirstOrDefault(t => t.QuoteID == id);
                    if (entity == null)
                    {
                        //return 404 not found
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Task with ID=" + id.ToString() + " is not found to delete.");
                    }
                    else
                    {
                        //delete the item
                        entities.tblTask.Remove(entity);
                        entities.SaveChanges();
                        //return 200 OK
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }

                }
            }
            catch (Exception ex) {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Put(int id, [FromBody] tblTask newTask)
        {
            try {
                using (TaskDBEntities entities = new TaskDBEntities())
                {
                    var entity = entities.tblTask.FirstOrDefault(t => t.QuoteID == id);

                    if (entity == null)
                    {
                        //return 404 not found
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Task with ID=" + id.ToString() + " is not found to update.");
                    }
                    else
                    {
                        entity.QuoteType = newTask.QuoteType;
                        entity.ContactID = newTask.ContactID;
                        entity.TaskDescription = newTask.TaskDescription;
                        entity.TaskDueDate = newTask.TaskDueDate;
                        entity.TaskType = newTask.TaskType;

                        entities.SaveChanges();

                        //return 200 OK
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
            }
            catch (Exception ex) 
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        private bool TaskExists(int id)
        {
            using (TaskDBEntities entities = new TaskDBEntities())
            {
                return entities.tblTask.Count(t => t.QuoteID == id) > 0;
            }
        }
    }
}
