using LibAPI.Models.ResponseResult;

namespace LibAPI.Extensions
{
    public static class ObjectExtension
    {
        public static ResponseResult<T> ToResponseResult<T>(this T sender, string message = "")
        {
            return new ResponseResult<T>(true, message, sender);
        }

        public static T GetResultResponse<T>(this ResponseResult<T> sender)
        {
            return sender.Value;
        }
        public static T GetResultResponse<T>(this ResponseResult sender)
        {
            return (T)sender.Value;
        }
    }
}