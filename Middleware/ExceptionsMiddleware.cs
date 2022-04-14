using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace LibAPI.Middleware
{
    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = e is HttpRequestException httpE
                    ? (int?)httpE.StatusCode ?? 500
                    : (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(
                    JsonConvert.SerializeObject(new
                    {
                        statusCode = context.Response.StatusCode,
                        message = e.Message,
                        isSuccess = false
                    }));
            }
        }
    }
}
