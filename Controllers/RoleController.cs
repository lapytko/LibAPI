using System;
using System.Threading.Tasks;
using LibAPI.Facade;
using LibAPI.Models.ResponseResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibAPI.Controllers
{ 
    [Authorize(Roles = "admin,superAdmin")]
    [Route("roles")]
    [ApiController]
    public class RoleController : Controller
    {
        private readonly AccountServiceFacade _accountServiceFacade;
        private readonly RoleServiceFacade _roleServiceFacade;

        public RoleController(AccountServiceFacade accountServiceFacade, RoleServiceFacade roleServiceFacade)
        {
            _accountServiceFacade = accountServiceFacade;
            _roleServiceFacade = roleServiceFacade;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                return ResponseResult.Success(await _roleServiceFacade.GetAll());
            }
            catch (Exception e)
            {
                return ResponseResult.Error(e);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(Guid id)
        {
            try
            {
                return ResponseResult.Success(await _roleServiceFacade.GetById(id));
            }
            catch (Exception e)
            {
                return ResponseResult.Error(e);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveRole(Guid id)
        {
            try
            {
                return ResponseResult.Success(await _roleServiceFacade.RemoveRole(id));
            }
            catch (Exception e)
            {
                return ResponseResult.Error(e);
            }
        }
    }
}