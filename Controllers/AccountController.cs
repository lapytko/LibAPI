using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LibAPI.Entities;
using LibAPI.Facade;
using LibAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;

namespace LibAPI.Controllers
{
    [Route("account")]
    [ApiController]
    public class AccountController : BaseUserController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly UserServiceFacade _userServiceFacade;
       

        public AccountController(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            UserServiceFacade userServiceFacade) : base(userManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _userServiceFacade = userServiceFacade;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<object> Login([FromBody] AccountModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName) ??
                       await _userManager.FindByEmailAsync(model.UserName);
            if (user is { IsDeleted: false } && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new(ClaimTypes.Name, user.UserName),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    _configuration["JWT:ValidIssuer"],
                    _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddDays(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    user = await _userServiceFacade.GetById(new Guid(user.Id))
                });
            }

            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpGet("token-valid")]
        public async Task<object> TokenValid()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]);

            try
            {
                string token = Request.Headers["Authorization"];
                if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    token = token.Substring("Bearer ".Length).Trim();
                }

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                var user = await _userManager.FindByNameAsync(User.Identity?.Name) ?? await _userManager
                    .FindByEmailAsync(User.Identity?.Name);

                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new(ClaimTypes.Name, user.UserName),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                userRoles = await _userManager.GetRolesAsync(user);

                authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var jwtToken = new JwtSecurityToken(
                    _configuration["JWT:ValidIssuer"],
                    _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddDays(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
                
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    expiration = jwtToken.ValidTo,
                    user = await _userServiceFacade.GetById(new Guid(user.Id)),
                });
            }
            catch
            {
                return ValidationProblem();
            }
        }
    }
}