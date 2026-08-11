namespace Microsoft.AspNetCore.Http
{
    public static class HttpContextExtensions
    {
        public static String GetBaseUrl(this HttpContext context)
        {
            return $"{context.Request.Scheme}://{context.Request.Host}";
        }
    }
}
