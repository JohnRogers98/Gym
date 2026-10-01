using Microsoft.AspNetCore.Mvc;

namespace Gym.BFF.Integration.Tests.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RandomGuidGeneratorController : ControllerBase
    {
        private const String GetUrl = "_fake-random-guid-generator";

        public static Uri GetUri { get; } = new Uri(GetUrl, UriKind.Relative);

        [HttpPost(GetUrl), HttpGet(GetUrl)]
        public async Task<ActionResult<RandomGuidResponse>> Endpoint(CancellationToken cancellationToken)
        {
            return base.Ok(new RandomGuidResponse(Guid.NewGuid()));
        }

        public record RandomGuidResponse(Guid Value);
    }
}
