using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace LibAPI.Models.ResponseResult
{
    [DefaultStatusCode(DefaultStatusCode)]
    public class ResponseSuccess : ObjectResult
    {
        public ResponseSuccess(object value) : base(value)
        {
            StatusCode = DefaultStatusCode;
        }
        
        private const int DefaultStatusCode = StatusCodes.Status200OK;
    }
}