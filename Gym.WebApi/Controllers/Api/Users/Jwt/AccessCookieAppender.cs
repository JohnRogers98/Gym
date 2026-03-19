namespace Gym.WebApi.Controllers.Api.Users.Jwt
{
    public interface IAccessCookieAppender
    {
        void AppendCookiesWithAccessToken(HttpContext httpContext, String accessToken);
    }

    public class AccessCookieAppender : IAccessCookieAppender
    {
        public void AppendCookiesWithAccessToken(HttpContext httpContext, String accessToken)
        {
            httpContext.Response.Cookies.Append("accessToken", accessToken, new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                Secure = this.IsRequestHttps(httpContext),
                SameSite = SameSiteMode.Unspecified
            });
        }

        private Boolean IsRequestHttps(HttpContext httpContext) => httpContext.Request.IsHttps;
    }
}
