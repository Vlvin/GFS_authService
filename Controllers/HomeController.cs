/*using Microsoft.AspNetCore.Http;
*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend.Helpers;
using Backend.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class HomeController(UserManager<AuthUser> userManager, JwtTokenProvider tokenProvider) : ControllerBase
    {
        private readonly JwtTokenProvider _tokenProvider = tokenProvider;
        private readonly UserManager<AuthUser> _userManager = userManager;

        [HttpGet("[action]")]
        public async Task<ActionResult<ProfileResponse>> Profile()
        {
            var Email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (Email == null) {
                return Unauthorized("Failed to read bearer token. ReLogin please");
            }
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null) {
                return Unauthorized("Failed to read bearer token. ReLogin please");
            }
            return Ok(new ProfileResponse(user.UserName, user.Email));
        }
        
    }
}

