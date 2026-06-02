using Microsoft.AspNetCore.Mvc;

namespace Gym.BFF.Controllers.Api
{
    [ApiController]
    [Route("api")]
    public class CheckSessionEndpoint : ControllerBase
    {
        [HttpGet("check-session")]
        public IActionResult CheckSession()
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
