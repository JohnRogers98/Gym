namespace Gym.BFF.DelegatingHandlers
{
    public class AddForwardHeadersHandler(IHttpContextAccessor _httpContextAccessor) : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                this.TryAddProtoHeader(_httpContextAccessor.HttpContext, request);
                this.TryAddHostHeader(_httpContextAccessor.HttpContext, request);
                this.TryAppendForHeader(_httpContextAccessor.HttpContext, request);
            }

            return await base.SendAsync(request, cancellationToken);
        }

        private const String XForwardedProto = "X-Forwarded-Proto";
        private const String XForwardedHost = "X-Forwarded-Host";
        private const String XForwardedFor = "X-Forwarded-For";

        private void TryAddProtoHeader(HttpContext context, HttpRequestMessage request)
        {
            var existingContextProto = context.Request.Headers[XForwardedProto].ToString();
            if (String.IsNullOrEmpty(existingContextProto) && request.Headers.Contains(XForwardedProto) is false)
            {
                request.Headers.TryAddWithoutValidation(XForwardedProto, context.Request.Scheme);
            }
        }

        private void TryAddHostHeader(HttpContext context, HttpRequestMessage request)
        {
            var existingContextHost = context.Request.Headers[XForwardedHost].ToString();
            if (String.IsNullOrEmpty(existingContextHost) && request.Headers.Contains(XForwardedHost) is false)
            {
                request.Headers.TryAddWithoutValidation(XForwardedHost, context.Request.Host.Value);
            }
        }

        private void TryAppendForHeader(HttpContext context, HttpRequestMessage request)
        {
            var existingContextFor = context.Request.Headers[XForwardedFor].ToString();
            var clientIp = context.Connection.RemoteIpAddress?.ToString();

            if (!String.IsNullOrEmpty(clientIp))
            {
                request.Headers.Remove(XForwardedFor);
                
                if (!String.IsNullOrEmpty(existingContextFor))
                    request.Headers.TryAddWithoutValidation(XForwardedFor, $"{existingContextFor}, {clientIp}");
                
                else
                    request.Headers.TryAddWithoutValidation(XForwardedFor, "sdfsdff");
            }
        }
    }
}
