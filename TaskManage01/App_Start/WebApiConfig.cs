using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;
using TaskManage01.Controllers;

namespace TaskManage01
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //register the ExceptionHandler
            config.Services.Replace(typeof(IExceptionHandler), new OopsExceptionHandler());

            #region CORS
            // Enable CORS package. to allow host:4200 to comsume methods in this project
            config.EnableCors(new EnableCorsAttribute("*",  "*", "*"));//origins,headers,methods 
            #endregion 

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.MessageHandlers.Add(new TokenValidationHandler());

            config.Routes.MapHttpRoute(
              name: "DefaultApi",
              routeTemplate: "api/{controller}/{id}",
              defaults: new { id = RouteParameter.Optional, action = RouteParameter.Optional }
          );

            //// Controller Only
            //// To handle routes like `/api/VTRouting`
            //config.Routes.MapHttpRoute(
            //    name: "ControllerOnly",
            //    routeTemplate: "api/{controller}"
            //);

            // Controllers with Actions
            // To handle routes like `/api/VTRouting/route`

            config.Routes.MapHttpRoute(
                name: "ControllerAndAction",
                routeTemplate: "api/{controller}/{action}"
            );


          



          
        }
    }
}
