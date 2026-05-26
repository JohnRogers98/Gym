using Gym.AuthorizationServer.Entities.Users;
using Gym.AuthorizationServer.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Gym.AuthorizationServer.Pages
{
    public class LoginModel(
        IUserByUsernameAndPasswordFinder _userByUsernameAndPassword) : PageModel
    {
        [BindProperty]
        [Required]
        public String? Username { get; set; }

        [BindProperty]
        [Required]
        public String? Password { get; set; }

        public IActionResult OnGet([FromQuery(Name = "req_id")] String requestId)
        {
            var hasRequestKey = base.HttpContext.Session.ContainsAuthorizeRequest(requestId);
            if (hasRequestKey is false)
                return this.RedirectToErrorPage("access_denied", "Request was not validated");

            base.TempData["req_id"] = requestId;

            return base.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userResult = await _userByUsernameAndPassword.FindAsync(Username!, Password!, CancellationToken.None);
            if(userResult.IsFailed)
                return Page();

            await this.GenerateCookieAsync(userResult.Value.Id);

            return base.RedirectToPage("/approve", new { req_id = base.TempData["req_id"] });
        }

        private async Task GenerateCookieAsync(String userId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddHours(1)
                });
        }

    }
}
