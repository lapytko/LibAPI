using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibAPI.Entities;
using LibAPI.Models;
using LibAPI.Models.ResponseResult;
using LibAPI.Service;
using LibAPI.Utils.DataFilterUtils.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibAPI.Facade
{
    public class UserServiceFacade
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserServiceFacade(IUserService userService, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        public async Task<IEnumerable<object>> GetAll() =>
            (await _userService.GetAll());

        public async Task<object> GetById(Guid id)
        {
            return (await _userService.GetById(id)).ToObject();
        }

        private async Task<ResponseResult> CreateUser(UserModel userModel)
        {
            var identityResult = await _userManager.CreateAsync(new ApplicationUser
            {
                UserName = userModel.UserName,
                Description = userModel.Description,
                Surname = userModel.Surname,
                Name = userModel.Name,
                Middle = userModel.Middle,
                
            }, userModel.Password);

            if (!identityResult.Succeeded)
                return new ResponseResult(false, identityResult.Errors.FirstOrDefault()?.Description);

            var user = _userManager.Users.FirstOrDefault(x => x.UserName == userModel.UserName);

            if (userModel.CanChange)
                await _userManager.AddToRolesAsync(user, new[] { "admin" });
            else
                await _userManager.AddToRolesAsync(user, new[] { "user" });

            // if (userModel.IsMobile)
            //     await _userManager.AddToRoleAsync(user, "mobile");
            // if (userModel.CanUnloadOrders)
            //     await _userManager.AddToRoleAsync(user, "unloadOrders");

            return new ResponseResult(true, "Saved user",
                await GetById(new Guid(user?.Id ?? throw new Exception("User is null"))));
        }

        private async Task<ResponseResult> UpdateUser(Guid id, UserModel userModel)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (!string.IsNullOrWhiteSpace(userModel.UserName))
            {
                user.UserName = userModel.UserName;
            }

            if (user == null) return new ResponseResult(false, "User null");

            user.Surname = userModel.Surname;
            user.Middle = userModel.Middle;
            user.Name = userModel.Name;
            user.Description = userModel.Description;
           
            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles.ToArray());

            if (userModel.CanChange)
                await _userManager.AddToRolesAsync(user, new[] { "admin" });
            else
                await _userManager.AddToRolesAsync(user, new[] { "user" });
            
            var userResult = await _userManager.UpdateAsync(user);

            if (string.IsNullOrWhiteSpace(userModel.Password)) return new ResponseResult(true, "");
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, resetToken, userModel.Password);

            return new ResponseResult(true, "");
        }

        public async Task<object> RemoveUser(IEnumerable<Guid> ids)
        {
            foreach (var user in ids.Select(id => _userManager.Users.FirstOrDefault(x => x.Id == id.ToString()))
                .Where(user => user != null))
            {
                await _userManager.DeleteAsync(user);
            }

            return true;
        }

        // public async Task<IEnumerable<ProductBalanceModel>> TryGetBalance(string token)
        // {
        //     return (await _fairApiService.GetBalance(token));
        // }

        public async Task<ResponseResult> SaveUser(UserModel userModel)
        {
            return userModel.Id == null
                ? await CreateUser(userModel)
                : await UpdateUser(new Guid(userModel.Id ?? string.Empty), userModel);
        }

        public async Task<bool> IsExists(string userName)
        {
            return await _userManager.Users.FirstOrDefaultAsync(x => userName.ToLower() == x.UserName.ToLower()
                                                                     && !x.IsDeleted) != null;
        }

        public async Task<(IEnumerable<TableUserModel>, int)> GetByFilter(FilterModel filterModel)
        {
            return await _userService.GetByFilter(filterModel);
        }
    }
}