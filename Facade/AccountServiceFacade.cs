using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibAPI.Entities;
using LibAPI.Models.ResponseResult;
using LibAPI.Service;
using Microsoft.AspNetCore.Identity;

namespace LibAPI.Facade
{
    public class AccountServiceFacade
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMailer _mailer;

        public AccountServiceFacade(RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IMailer mailer)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mailer = mailer;
        }

        public IEnumerable<ApplicationRole> GetRoles()
        {
            return _roleManager.Roles;
        }

        public Task<IdentityResult> AddRole(ApplicationRole role)
        {
            return _roleManager.CreateAsync(role);
        }

        // public async IAsyncEnumerable<object> GetUsers()
        // {
        //     var users = await _userManager.Users
        //         .ToListAsync();
        //     foreach (var user in users)
        //     {
        //         var role = await _userManager.GetRolesAsync(user);
        //         if (role.FirstOrDefault() == "Admin") continue;
        //
        //         yield return new
        //         {
        //             Id = user.Id,
        //             Fio = user.Fio,
        //             Password = user.PasswordEncrypt != null
        //                 ? Encrypter.DecryptString(user.PasswordEncrypt)
        //                 : null,
        //             Role = role.FirstOrDefault(),
        //             UserName = user.UserName,
        //         };
        //     }
        // }

        // public async Task<UserModel> GetUser(string userId)
        // {
        //     var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);
        //     return user != null
        //         ? new UserModel()
        //         {
        //             Id = user.Id,
        //             UserName = user.UserName,
        //             Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
        //         }
        //         : null;
        // }

        public async Task<string> GetRoleByUser(string name)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.UserName == name);
            return (await _userManager.GetRolesAsync(user)).FirstOrDefault();
        }

        public string GetUserIdByName(string name)
        {
            return _userManager.Users.FirstOrDefault(x => x.UserName == name)?.Id;
        }
        
        // public async Task<object> CreateUser(UserModel userModel)
        // {
        //     var password = GenerateRandomPassword();
        //     //userModel.Password = password;
        //
        //     userModel.UserName = userModel.UserName;
        //
        //     var identityResult = await _userManager.CreateAsync(new ApplicationUser()
        //     {
        //         Email = "example@gmail.com",
        //         UserName = userModel.UserName
        //     }, userModel.Password);
        //
        //     if (identityResult.Succeeded)
        //     {
        //         var user = _userManager.Users.FirstOrDefault(x => x.UserName == userModel.UserName);
        //         var roleResult = await _userManager.AddToRoleAsync(user, userModel.Role);
        //
        //         return identityResult.Succeeded && roleResult.Succeeded
        //             ? new ResponseResult(true, "Saved user", new
        //             {
        //                 user?.UserName,
        //                 password
        //             })
        //             : new ResponseResult(false, "Role don't created");
        //     }
        //
        //     return new ResponseResult(false, identityResult.Errors.FirstOrDefault()?.Description);
        // }

        // public async Task<bool> UpdateUser(UserModel userModel)
        // {
        //     var user = await _userManager.FindByIdAsync(userModel.Id);
        //
        //     if (user != null)
        //     {
        //         var roles = await _userManager.GetRolesAsync(user);
        //         await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
        //
        //         var roleResult = await _userManager.AddToRoleAsync(user, userModel.Role);
        //
        //         var userResult = await _userManager.UpdateAsync(user);
        //         return roleResult.Succeeded && userResult.Succeeded;
        //     }
        //
        //     return false;
        // }

        public async Task<ResponseResult> RenameUser(string id, string name)
        {
            var existUser = _userManager.Users.FirstOrDefault(x => x.UserName == name);
            if (existUser != null)
                return new ResponseResult(false, "Username exist", 1488);

            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                var result = await _userManager.SetUserNameAsync(user, name);
                return new ResponseResult(result.Succeeded, "POST", result.Errors);
            }

            return new ResponseResult(false, "Cannot find by id");
        }

        public async Task<object> RemoveUser(string userId)
        {
            try
            {
                var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles);
                await _userManager.DeleteAsync(user);
                return new ResponseResult(true, "Removed user");
            }
            catch (Exception e)
            {
                return new ResponseResult(false, e.Message);
            }
        }

        public static string GenerateRandomPassword(PasswordOptions opts = null)
        {
            if (opts == null)
                opts = new PasswordOptions()
                {
                    RequiredLength = 8,
                    RequiredUniqueChars = 4,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireNonAlphanumeric = false,
                    RequireUppercase = true
                };

            var randomChars = new[]
            {
                "ABCDEFGHJKLMNOPQRSTUVWXYZ", // uppercase 
                "abcdefghijkmnopqrstuvwxyz", // lowercase
                "0123456789", // non-alphanumeric
            };
            var rand = new Random(Environment.TickCount);
            var chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (var i = chars.Count;
                i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars;
                i++)
            {
                var rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }

        // public async Task<object> ResetPassword(string userId)
        // {
        //     try
        //     {
        //         var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
        //         var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        //         var newPassword = GenerateRandomPassword();
        //         var resetPasswordAsync = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
        //         user.PasswordEncrypt = Encrypter.EncryptString(newPassword);
        //         await _userManager.UpdateAsync(user);
        //         return new ResponseResult(true, "Password cleared");
        //     }
        //     catch (Exception e)
        //     {
        //         return new ResponseResult(false, e.Message);
        //     }
        // }
    }
}