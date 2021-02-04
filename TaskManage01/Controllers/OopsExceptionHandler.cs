﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskManage01.Controllers
{
    using System.Diagnostics;
    using System.Net;  
    using System.Net.Http;
    using System.Text;
    using System.Threading;  
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.ExceptionHandling;  
    using System.Web.Http.Results;

    public class OopsExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            context.Result = new TextPlainErrorResult
            {
                //If you want to return the actual error message
                //Content = context.Exception.Message 
                Request = context.ExceptionContext.Request,
                Content = "Oops! Sorry! Something went wrong." +
                          "Please contact support@contoso.com so we can try to fix it."
            };
        }

        private class TextPlainErrorResult : IHttpActionResult
        {
            public HttpRequestMessage Request { get; set; }

            public string Content { get; set; }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                HttpResponseMessage response =
                                 new HttpResponseMessage(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(Content);
                response.RequestMessage = Request;
                return Task.FromResult(response);
            }
        }
    }

}