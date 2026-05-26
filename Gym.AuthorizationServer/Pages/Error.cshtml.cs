using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gym.AuthorizationServer.Pages
{
    public class ErrorModel : PageModel
    {
        public required String Error { get; set; }
        public String? Description { get; set; }

        public void OnGet(String error, String? description)
        {
            Error = error;
            Description = description;
        }
    }
}
