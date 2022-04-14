using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace LibAPI.Models.ResponseResult
{
    [DefaultStatusCode(DefaultStatusCode)]
    public class ResponseError : ObjectResult
    {
        public ResponseError(string message) : base(new {message})
        {
            StatusCode = DefaultStatusCode;
        }

        public ResponseError(Exception e) : this(e.Message)
        {
        }
        
        private const int DefaultStatusCode = StatusCodes.Status500InternalServerError;
    }
}