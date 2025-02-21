/*using Microsoft.AspNetCore.Http;
*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class HomeController(UserManager<AuthUser> userManager) : ControllerBase
    {
        private readonly UserManager<AuthUser> _userManager = userManager;

        [HttpGet("[action]")]
        public async Task<ActionResult<ProfileResponse>> Me()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                return Unauthorized("Failed to get email, bad token");
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Unauthorized("Failed to get user, bad token");
            return Ok(new ProfileResponse(user.Id, user.UserName, user.Email));
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<ProfileResponse>> Profile(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("No such user");
            }
            return Ok(new ProfileResponse(user.Id, user.UserName, user.Email));
        }

    }
}

