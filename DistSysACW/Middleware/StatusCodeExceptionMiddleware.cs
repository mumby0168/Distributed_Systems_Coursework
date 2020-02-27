using System;
using System.Threading.Tasks;
using DistSysACW.Exceptions;
using Microsoft.AspNetCore.Http;

namespace DistSysACW.Middleware
{
    public class StatusCodeExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (HttpStatusCodeException e)
            {
                context.Response.StatusCode = (int) e.StatusCode;
                await context.Response.WriteAsync(e.Message);
            }
        }
    }
}