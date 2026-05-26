using System.ComponentModel;

namespace System.Net.Http;

[EditorBrowsable(EditorBrowsableState.Never)]
internal static class HttpResponseMessageExtension
{
    public static HttpResponseMessage EnsureRedirectStatusCode(this HttpResponseMessage message)
    {
        if (message.StatusCode is not HttpStatusCode.Redirect)
            throw new HttpRequestException("Authorize response has no 302 status code");

        return message;
    }
}
