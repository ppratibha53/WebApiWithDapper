using Microsoft.AspNetCore.Diagnostics;
using System;
using System.Net;
using WebApplication2.Contracts;
using WebApplication2.Entities;

namespace WebApplication2.Extensions
{
    public static  class ExceptionMiddlewareExtensions
    {
       public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerManager logger) 
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async Context =>
                {
                    Context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    Context.Response.ContentType = "application/json";
                    var contextFeature = Context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.LogError($"Something went wrong: {contextFeature.Error}");
                        await Context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = Context.Response.StatusCode,
                            Message = "Internal Server Error."
                        }.ToString());
                    }
                });
            });
        }
    } 
}
