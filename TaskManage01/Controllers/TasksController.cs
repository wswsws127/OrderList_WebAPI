using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using TaskDataAccess;

namespace TaskManage01.Controllers
{
    //[Authorize]
    //[BasicAuthentication]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TasksController : ApiController
    {

        //[NotImplExceptionFilter]
        //public tblTask GetNewTask(int id)
        //{
        //    throw new NotImplementedException("This method is not implemented");
        //}

        
        [HttpGet]
       //[NotImplExceptionFilter]
        public IEnumerable<tblTask> Get()
        {
            using (TaskDBEntities entities = new TaskDBEntities()) 
            {
                //throw new NotImplementedException("This method is not implemented");
                return entities.tblTask.ToList();
            }
        }

        [HttpGet]
        
       // public HttpResponseMessage Get(int id)
         public tblTask Get(int id)
        {
            using (TaskDBEntities entities = new TaskDBEntities())
            {
                var entity= entities.tblTask.FirstOrDefault(t => t.QuoteID==id);
                

                if (entity != null)
                {
                    //return 200 OK
                    //return Request.CreateResponse(HttpStatusCode.OK, entity);
                    return entity;
                }
                else
                {
                    var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(string.Format("No task with ID = {0}", id)),
                        ReasonPhrase = "Task ID Not Found"
                    };
                    throw new HttpResponseException(resp);
                    //return 404 not found
                    //return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Task with ID=" + id.ToString() + " is not found.");
                }
            }
        }

        [HttpPost]
       
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

        [HttpDelete]
        //[Route("DeleteTaskDetails")]
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

        [HttpPut]
        //[Route("UpdateTaskDetails")]
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
