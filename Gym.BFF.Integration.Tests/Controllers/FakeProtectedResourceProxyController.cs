using Gym.BFF.Integration.Tests.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Gym.BFF.Integration.Tests.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class FakeProtectedResourceProxyController(
        IHttpClientFactory _httpClientFactory,
        IOptions<ProtectedResourceOptions> _protectedResourceOptions) : ControllerBase
    {
        private const String GetUrl = "_fake-protected-resource-proxy";

        public static Uri GetUri { get; } = new Uri(GetUrl, UriKind.Relative);

        [HttpPost(GetUrl)]
        public async Task<IActionResult> Endpoint(ProtectedResourceProxyRequest request, CancellationToken cancellationToken)
        {
            if (this.IsSessionKeyPresent() is false)
                return Unauthorized();

            using var _protectedResourceClient = _httpClientFactory.CreateClient(_protectedResourceOptions.Value.ClientName);
            using var proxyRequestMessage = await this.CreateProxyRequestAsync(request.RelativeUrl, httpMethod: request.Method, cancellationToken: cancellationToken);
            using var response = await _protectedResourceClient.SendAsync(proxyRequestMessage, cancellationToken);

            return await this.CreateProxyResponseAsync(response, cancellationToken);
        }
    }

    public record ProtectedResourceProxyRequest
    {
        public required String RelativeUrl { get; init; }
        public required String Method { get; init; }
        public required String JsonSerializedBody { get; init; }
    }
}
