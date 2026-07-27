using Microsoft.AspNetCore.Mvc;

namespace Gym.AuthorizationServer.Integration.Tests.Middleware
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public sealed class ServerInfoController : Controller
    {
        private const String GetUrl = "_testing/info";

        public static Uri GetInfoUri { get; } = new Uri(GetUrl, UriKind.Relative);

        [HttpGet]
        [Route(GetUrl, Name = "InfoEndpoint")]
        public ActionResult<ServerInfo> InfoEndpoint()
        {
            ServerInfo serverInfo = new() 
            { 
                Host = Request.Host.ToString(),
                Scheme = Request.Scheme 
            };
            
            return Ok(serverInfo);
        }
    }

    public class ServerInfo
    {
        public String? Host { get; set; }
        public String? Scheme { get; set; }
    }
}
