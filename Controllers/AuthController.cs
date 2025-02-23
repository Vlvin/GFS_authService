/*using Microsoft.AspNetCore.Http;
*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend.Helpers;
using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController(UserManager<AuthUser> userManager, JwtTokenProvider tokenProvider, SignInManager<AuthUser> signInManager) : ControllerBase
    {
        private readonly JwtTokenProvider _tokenProvider = tokenProvider;
        private readonly UserManager<AuthUser> _userManager = userManager;

        private readonly SignInManager<AuthUser> _signInManager = signInManager;

        [HttpPost("[action]")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) // Unexisted User
            {
                return Unauthorized("Email or password is incorrect (spoiler Email)");
            }
            else
            {
                var userIsAuthenticated = await _userManager.CheckPasswordAsync(user, request.Password);
                if (!userIsAuthenticated)
                {
                    return Unauthorized("Email or password is incorrect (spoiler Password)");
                }
            }
            var userId = user.Id;
            var userEmail = user.Email;
            var token = _tokenProvider.GenerateToken(userId, userEmail);
            return Ok(new LoginResponse(token));
        }
        [HttpPost("[action]")]
        public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null) // Unexisted User
            {
                return Conflict("User with this Email already exists");
            }
            user = new AuthUser
            {
                Email = request.Email,
                UserName = request.Username,
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var sb = new StringBuilder();
                sb.Append("Failed to create user: ");
                sb.Append($"{result.Errors.Count()}\n");

                result.Errors.Select(x => sb.Append($"{x.Description.Length}"));
                result.Errors.Select(x => sb.Append($"{x.Description};\n"));

                return Conflict(sb.ToString());
            }
            user = await _userManager.FindByEmailAsync(request.Email); // renew user
            if (user == null)
            {
                return Conflict("Failed to renew user");
            }
            var userId = user.Id;
            var userEmail = user.Email;
            var token = _tokenProvider.GenerateToken(userId, userEmail);
            return Ok(new RegisterResponse(token));
        }
        [HttpGet("[action]")]
        public async Task<ActionResult<AuthorizeResponse>> Authorize()
        {
            var Email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (Email == null)
            {
                return Ok(new AuthorizeResponse(false, "", ""));
            }
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return Ok(new AuthorizeResponse(false, "", ""));
            }
            return Ok(new AuthorizeResponse(true, user?.UserName, user?.Email));
        }
    }
}

