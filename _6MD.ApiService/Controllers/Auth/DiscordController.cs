using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Client.AspNetCore;

namespace _6MD.ApiService.Controllers.Auth
{
    [Route("auth/[controller]")]
    public class DiscordController : Controller
    {
        // Start the OAuth flow with Discord
        [HttpGet("login")]
        public IActionResult Login()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "/auth/Discord/callback"
            };

            return Challenge(properties, OpenIddictClientAspNetCoreDefaults.AuthenticationScheme);
        }

        // Handle the callback from Discord
        [HttpGet("callback")]
        public async Task<IActionResult> Callback()
        {
            var result = await HttpContext.AuthenticateAsync(OpenIddictClientAspNetCoreDefaults.AuthenticationScheme);

            if (!result.Succeeded)
                return BadRequest("OAuth authentication failed.");

            var claims = result.Principal?.Claims;
            return Ok(claims?.Select(c => new { c.Type, c.Value }));
        }
    }
}
