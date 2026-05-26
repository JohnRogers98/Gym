using Microsoft.AspNetCore.Mvc;

namespace Gym.AuthorizationServer.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static IActionResult RedirectToErrorPage(this ControllerBase controllerBase, String error, String? description = null)
        {
            return controllerBase.RedirectToPage("/error", new { error, description });
        }

        public static IActionResult CallbackClientWithError(this ControllerBase controllerBase, String redirectUri, String error, String? description = null, String? state = null)
        {
            var queryParams = new QueryString();

            queryParams = queryParams.Add("error", error);

            if (String.IsNullOrWhiteSpace(description) is false)
                queryParams = queryParams.Add("error_description", description);

            if (String.IsNullOrWhiteSpace(state) is false)
                queryParams = queryParams.Add("state", state);

            return controllerBase.Redirect($"{redirectUri}{queryParams.Value}");
        }
    }
}
