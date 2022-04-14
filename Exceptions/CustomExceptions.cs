using System.Net;
using System.Net.Http;

namespace LibAPI.Exceptions
{
    // public class NonAllowDocumentException : Exception
    // {
    //     public NonAllowDocumentException() : base("Неверный документ для данного запроса")
    //     {
    //     }
    // }

    public class ItemNotFoundException : HttpRequestException
    {
        public ItemNotFoundException(HttpStatusCode code = HttpStatusCode.NotFound, string message = null) : base(
            message ?? "Данные не найдены", null, code)
        {
        }
    }
}
