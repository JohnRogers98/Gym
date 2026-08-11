using Microsoft.AspNetCore.Http;

namespace Gym.AuthorizationServer.Integration.Tests.Fakes
{
    public class FakeHttpContextAccessor : IHttpContextAccessor
    {
        private HttpContext? _httpContext;
        private readonly String _baseUrl;

        public FakeHttpContextAccessor(string baseUrl = "https://localhost")
        {
            _baseUrl = baseUrl;
        }

        public HttpContext? HttpContext
        {
            get
            {
                if (_httpContext is null)
                    _httpContext = this.CreateHttpContext();

                return _httpContext;
            }
            set => _httpContext = value;
        }

        private HttpContext CreateHttpContext()
        {
            var context = new DefaultHttpContext();
            context.Request.Scheme = "https";
            context.Request.Host = new HostString(_baseUrl);
            context.Request.PathBase = "";
            return context;
        }
    }
}
