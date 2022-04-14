using LibAPI.Entities;

namespace LibAPI.Extensions
{
    public static class ApplicationUserExtensions
    {
        public static object GetFrontObject(this ApplicationUser user)
        {
            return new
            {
                user.Id,
                user.Email,
                user.UserName
            };
        }
    }
}