using Gym.BFF.Options;
using Microsoft.Extensions.Options;

namespace Gym.BFF.Middlewares
{
    public class StaticHeaderCheckForCorsImposing
    {
        private readonly RequestDelegate _next;
        private readonly StaticHeaderCheckOptions _options;

        public StaticHeaderCheckForCorsImposing(RequestDelegate next, IOptions<StaticHeaderCheckOptions> options)
        {
            _next = next;
            _options = options.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path;

            var isExcludedFromCheck = !_options.Enabled 
                || _options.GetExcludedPathStrings().Contains(path) 
                || _options.GetExcludedPathStrings().Any(path.StartsWithSegments);

            if (isExcludedFromCheck is false && !context.Request.Headers.ContainsKey("X-Static-Header"))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Missing X-Static-Header");
                return;
            }
            await _next(context);
        }
    }
}
