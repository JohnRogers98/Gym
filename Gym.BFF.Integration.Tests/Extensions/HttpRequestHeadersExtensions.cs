namespace System.Net.Http.Headers;

public static class HttpRequestHeadersExtensions
{
    public static void AddXStaticHeader(this HttpRequestHeaders headers) => headers.Add("X-Static-Header", "1");
}
