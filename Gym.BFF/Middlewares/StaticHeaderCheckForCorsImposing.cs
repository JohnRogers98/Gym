namespace Gym.BFF.Middlewares
{
    public class StaticHeaderCheckForCorsImposing
    {
        private readonly RequestDelegate _next;
        private readonly HashSet<PathString> _excludedPaths;

        public StaticHeaderCheckForCorsImposing(RequestDelegate next)
        {
            _next = next;

            _excludedPaths = new HashSet<PathString>
            {
                "/login",
                "/callback",
                "/logout"
            };
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path;

            var isExcludedFromCheck = _excludedPaths.Contains(path) || _excludedPaths.Any(path.StartsWithSegments);

            if (isExcludedFromCheck is false && !context.Request.Headers.ContainsKey("X-Static-Header"))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Missing X-Static-Header header");
                return;
            }
            await _next(context);
        }
    }
}
