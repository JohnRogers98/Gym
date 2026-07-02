using Gym.BFF.Options;
using System.Net.Http.Headers;
using System.Text;

namespace Microsoft.AspNetCore.Mvc;

public static class ControllerBaseExtensions
{
    extension(ControllerBase controllerBase)
    {
        public async Task<HttpRequestMessage> CreateProxyRequestAsync(String url, Boolean enableBuffering = false, CancellationToken cancellationToken = default)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage
            {
                Method = new HttpMethod(controllerBase.Request.Method),
                RequestUri = new Uri(url, UriKind.Relative)
            };

            //Header
            var accessToken = controllerBase.User.FindFirst(ExtendedClaimTypes.AccessToken)?.Value;
            if(!String.IsNullOrEmpty(accessToken))
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            //Body
            if (controllerBase.Request.ContentLength > 0)
            {
                if(enableBuffering) 
                    controllerBase.Request.EnableBuffering();

                var body = await new StreamReader(controllerBase.Request.Body).ReadToEndAsync(cancellationToken);

                if (enableBuffering)
                    controllerBase.Request.Body.Position = 0;

                if (!String.IsNullOrEmpty(body))
                {
                    var contentType = controllerBase.Request.ContentType ?? "application/json";
                    var mediaType = MediaTypeHeaderValue.Parse(contentType);
                    httpRequestMessage.Content = new StringContent(body, Encoding.UTF8, mediaType);
                }
            }

            return httpRequestMessage;
        }

        public async Task<ActionResult> CreateProxyResponseAsync(HttpResponseMessage sourceResponse, CancellationToken cancellationToken = default)
        {
            controllerBase.Response.StatusCode = (Int32)sourceResponse.StatusCode;
            
            var contentType = sourceResponse.Content.Headers.ContentType?.ToString() ?? "application/json";
            controllerBase.Response.ContentType = contentType;

            await using var sourceStream = await sourceResponse.Content.ReadAsStreamAsync(cancellationToken);
            await sourceStream.CopyToAsync(controllerBase.Response.Body, cancellationToken);

            return new EmptyResult();
        }

        public Boolean IsAccessTokenPresent()
        {
            var accessToken = controllerBase.User.FindFirst(ExtendedClaimTypes.AccessToken)?.Value;

            if (String.IsNullOrEmpty(accessToken))
                return false;
            return true;
        }

    }
}
