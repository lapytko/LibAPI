using System.Web;

namespace LibAPI.Extensions
{
    public static class StringExtension
    {
        public static string ToUrlEncode(this string str)
        {
            return HttpUtility.UrlEncode(str);
        }
    }
}