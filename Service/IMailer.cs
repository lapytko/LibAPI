using System.Threading.Tasks;

namespace LibAPI.Service
{
    public interface IMailer
    {
        Task SendEmailAsync(string email, string message);
    }
}