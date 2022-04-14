using System;
using Microsoft.AspNetCore.Mvc;

namespace LibAPI.Models.ResponseResult
{
    public class ResponseResult<T> : ResponseResult
    {
        public new T Value { get; set; }
        public ResponseResult(bool status, string message, T value = default) : base(status, message, value)
        {
            Value = value;
        }
    }

    public class ResponseResult
    {
        public string Message { get; set; }
        public object Value { get; set; }
        public bool IsSuccess { get; set; }

        public ResponseResult(bool status, string message, object value = default)
        {
            IsSuccess = status;
            Message = message;
            Value = value;
        }

        public static IActionResult Error(string mes)
        {
            return new ResponseError(mes);
        }

        public static IActionResult Error(Exception e)
        {
            return new ResponseError(e);
        }
        
        public static IActionResult Success<T>(T value = default)
        {
            return new ResponseSuccess(value);
        }

        public IActionResult Success()
        {
            return new ResponseSuccess(Value);
        }
    }
}