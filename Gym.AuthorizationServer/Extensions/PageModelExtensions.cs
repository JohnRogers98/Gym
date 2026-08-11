using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gym.AuthorizationServer.Extensions
{
    public static class PageModelExtensions
    {
        public static IActionResult RedirectToErrorPage(this PageModel pageModel, String error, String? description = null)
        {
            return pageModel.RedirectToPage("/error", new { error, description });
        }

        public static IActionResult CallbackClientWithError(this PageModel pageModel, String redirectUri, String error, String? description = null, String? state = null)
        {
            var queryParams = new QueryString();

            queryParams = queryParams.Add("error", error);

            if (String.IsNullOrWhiteSpace(description) is false)
                queryParams = queryParams.Add("error_description", description);

            if (String.IsNullOrWhiteSpace(state) is false)
                queryParams = queryParams.Add("state", state);

            return new RedirectResult($"{redirectUri}{queryParams.Value}");
        }
    }
}
