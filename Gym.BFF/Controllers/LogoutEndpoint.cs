using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Gym.BFF.Controllers
{
    [ApiController]
    public class LogoutEndpoint : ControllerBase
    {
        [HttpPost("logout")]
        public async Task<IActionResult> HandleAsync()
        {
            //client-side session
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //server-side session
            HttpContext.Session.Clear();
            
            return Ok();
        }
    }
}
