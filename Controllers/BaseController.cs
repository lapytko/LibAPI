using System.Security.Claims;
using System.Threading.Tasks;
using LibAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace LibAPI.Controllers
{
    [Authorize]
    [ApiController]
    // public class BaseController : Controller
    // {
    //     protected const string TokenHeader = "auth-token";
    // }

    public class BaseUserController:Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public BaseUserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [NonAction]
        protected async Task<string> UserId()
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userManager.FindByNameAsync(userName);
            return user.Id;
        }
    }
}