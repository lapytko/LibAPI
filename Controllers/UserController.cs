using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookLibWebApi.Models;
using LibAPI.Extensions;
using LibAPI.Facade;
using LibAPI.Models;
using LibAPI.Models.ResponseResult;
using LibAPI.Utils.DataFilterUtils.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibAPI.Controllers
{
    [Authorize(Roles = "admin,superAdmin")]
    [Route("users")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserServiceFacade _userServiceFacade;

        public UserController(UserServiceFacade userServiceFacade)
        {
            _userServiceFacade = userServiceFacade;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                return ResponseResult.Success(await _userServiceFacade.GetAll());
            }
            catch (Exception e)
            {
                return ResponseResult.Error(e);
            }
        }
        
        [Authorize]
        [HttpPost("with-filter")]
        public async Task<ResponseResult<PaginationModel<IEnumerable<TableUserModel>>>> GetUsers(
            [FromBody] FilterModel filterModel)
        {
            return (await _userServiceFacade.GetByFilter(filterModel)).ToPaginationModel().ToResponseResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                return ResponseResult.Success(await _userServiceFacade.GetById(id));
            }
            catch (Exception e)
            {
                return ResponseResult.Error(e);
            }
        }

        [HttpPost("is-exists")]
        public async Task<IActionResult> IsExistUser([FromBody] string userName)
        {
            try
            {
                return ResponseResult.Success(await _userServiceFacade.IsExists(userName));
            }
            catch (Exception e)
            {
                return ResponseResult.Error(e);
            }
        }

        /// <summary>
        /// Create / Update user
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveUser([FromBody] UserModel userModel)
        {
            return ResponseResult.Success(await _userServiceFacade.SaveUser(userModel));
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveUser([FromBody] List<Guid> ids)
        {
            try
            {
                return ResponseResult.Success(await _userServiceFacade.RemoveUser(ids));
            }
            catch (Exception e)
            {
                return ResponseResult.Error(e);
            }
        }
    }
}