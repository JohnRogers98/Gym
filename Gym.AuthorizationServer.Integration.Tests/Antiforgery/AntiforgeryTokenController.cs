using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mime;

namespace Gym.AuthorizationServer.Integration.Tests.Antiforgery;

[ApiExplorerSettings(IgnoreApi = true)]
public sealed class AntiforgeryTokenController : Controller
{
    private const String GetUrl = "_testing/get-xsrf-token";

    public static Uri GetTokensUri { get; } = new Uri(GetUrl, UriKind.Relative);

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json, Type = typeof(AntiforgeryTokens))]
    [Route(GetUrl, Name = "GetAntiforgeryTokens")]
    public IActionResult GetAntiforgeryTokens(
        [FromServices] IAntiforgery antiforgery,
        [FromServices] IOptions<AntiforgeryOptions> options)
    {
        ArgumentNullException.ThrowIfNull(antiforgery);
        ArgumentNullException.ThrowIfNull(options);

        AntiforgeryTokenSet tokens = antiforgery.GetTokens(HttpContext);

        var model = new AntiforgeryTokens()
        {
            CookieName = options.Value!.Cookie!.Name!,
            CookieValue = tokens.CookieToken!,
            FormFieldName = options.Value.FormFieldName,
            HeaderName = tokens.HeaderName!,
            RequestToken = tokens.RequestToken!,
        };

        return Json(model);
    }
}