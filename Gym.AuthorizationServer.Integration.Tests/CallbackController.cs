using Microsoft.AspNetCore.Mvc;

namespace Gym.AuthorizationServer.Integration.Tests;

[ApiExplorerSettings(IgnoreApi = true)]
public sealed class CallbackController : Controller
{
    private const String GetUrl = "_testing/callback";

    public static Uri GetCallbackUri { get; } = new Uri(GetUrl, UriKind.Relative);

    [HttpPost]
    [Route(GetUrl, Name = "CallbackEndpoint")]
    public IActionResult CallbackEndpoint()
    {
        return Ok();
    }
}