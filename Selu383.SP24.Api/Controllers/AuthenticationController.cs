using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Selu383.SP24.Api.Data;
using Selu383.SP24.Api.Features.Users;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace Selu383.SP24.Api.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UserDto>> Me() 
        {
            var username = GetCurrentUserName(User);

            if (string.IsNullOrEmpty(username)) 
            {
                return Unauthorized();
            }

            var resultDto = await GetUserDto(userManager.Users).SingleAsync
                (x => x.UserName == username);
            
            return Ok(resultDto);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto) 
        {
            var user = await userManager.FindByNameAsync(loginDto.UserName);
            
            if (user == null) 
            {
                return BadRequest("Invalid username");
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, true);
            
            if (!result.Succeeded) 
            {
                return BadRequest("Invalid password");
            }

            await signInManager.SignInAsync(user, false);

            var resukltDto = await GetUserDto(userManager.Users).SingleAsync(x => x.UserName == user.UserName);

            return Ok(resukltDto);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult> Logout() 
        {
            await signInManager.SignOutAsync();

            return Ok();
        }

        private static IQueryable<UserDto> GetUserDto(IQueryable<User> users) 
        {
            return users.Select(x => new UserDto
            {
                Id = x.Id,
                UserName = x.UserName!,
                Roles = x.Roles.Select(y => y.Role!.Name).ToArray()!
            });
        }

        private string? GetCurrentUserName(ClaimsPrincipal claimsPrincipal) 
        {
            return claimsPrincipal.Identity?.Name;
        } 
    }
}
