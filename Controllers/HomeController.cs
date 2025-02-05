/*using Microsoft.AspNetCore.Http;*/
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
      [HttpGet]
      public IActionResult Index(int age) {
        return Ok($"You are {age} years old");
      }
    }
}
