using Microsoft.AspNetCore.Mvc;

namespace Gym.BFF.Integration.Tests.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class MissingStaticHeaderController : ControllerBase
    {
        private const String GetUrl = "_missing-x-static-header";

        public static Uri GetUri { get; } = new Uri(GetUrl, UriKind.Relative);

        [HttpGet(GetUrl)]
        public IActionResult Endpoint()
        {
            return base.Ok();
        } 
    }
}
