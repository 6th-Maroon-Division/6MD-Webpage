using _6MD.AuthServer.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using System.Text.Json;

namespace _6MD.ApiService.Controllers
{
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            string? value = User.FindFirst("permissions")?.Value;
            if (!User.HasAudience("API"))
            {
                return Unauthorized("Not Working");
            }
            return Ok("Hello, world!");
        }

        [HttpGet("debug-token")]
        public IActionResult DebugToken()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Ok(claims);
        }
    }
}
