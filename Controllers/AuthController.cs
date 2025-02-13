/*using Microsoft.AspNetCore.Http;
*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend.Helpers;
using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController(UserManager<AuthUser> userManager, JwtTokenProvider tokenProvider) : ControllerBase
    {
        private readonly JwtTokenProvider _tokenProvider = tokenProvider;
        private readonly UserManager<AuthUser> _userManager = userManager;

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Authenticate(string Email, string Password)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null) // Unexisted User
            {
                user = new AuthUser {
                    UserName = Email.Split('@')[0], 
                    Email = Email 
                };
                var result = await _userManager.CreateAsync(user, Password);
                if (!result.Succeeded)
                    return Conflict($"Something went wrong: {result.Errors.First().Description}");
                // return Unauthorized("No account with this email exists");
            } else {
                var userIsAuthenticated = await _userManager.CheckPasswordAsync(user, Password);
                if (!userIsAuthenticated)
                {
                    return Unauthorized("Invalid credentials");
                }
            }
            var userId = user.Id;
            var userEmail = user.Email;
            var token = _tokenProvider.GenerateToken(userId, userEmail);
            return Ok($"{token}");
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Authorize()
        {
            var token = Request.Headers.Authorization;
            Console.WriteLine(token);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized("Somehow");
            return Ok($"Authorized as {user?.Email}");
        }
    }
}
