using Microsoft.AspNetCore.Mvc;

namespace Gym.BFF.Controllers
{
    [ApiController]
    public class CheckSessionEndpoint : ControllerBase
    {
        [HttpGet("check-session")]
        public IActionResult Handle()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return Ok(new
                {
                    authenticated = true
                });
            }

            return Ok(new
            {
                authenticated = false
            });
        }
    }
}
