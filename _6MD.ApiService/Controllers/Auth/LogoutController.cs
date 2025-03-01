using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _6MD.ApiService.Controllers.Auth
{
    [Route("auth/[controller]")]
    public class LogoutController : Controller
    {
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(); // Clear authentication cookies
            return Ok(new { message = "Logged out successfully" });
        }
    }

}
