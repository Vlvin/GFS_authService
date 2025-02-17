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

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class HomeController(UserManager<AuthUser> userManager, JwtTokenProvider tokenProvider) : ControllerBase
    {
        private readonly JwtTokenProvider _tokenProvider = tokenProvider;
        private readonly UserManager<AuthUser> _userManager = userManager;

        [HttpGet]
        public async Task<IActionResult> Me()
        {
            return Ok($"{User.Identity?.Name ?? "Fuck you"}");
        }
        
    }
}

