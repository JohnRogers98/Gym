using Gym.AuthorizationServer.Entities.GrantCodes;
using Gym.AuthorizationServer.Extensions;
using Gym.AuthorizationServer.Queries;
using Gym.AuthorizationServer.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Gym.AuthorizationServer.Pages
{
    public class ApproveModel(IGrantCodeGenerator _grantCodeGenerator,
        IGrantCodeRepository _grantCodeRepository,
        IConsentEvaluationService _consentEvaluationService,
        IUpsertUserConsentService _upsertUserConsentService) : PageModel
    {
        [BindProperty]
        public List<ScopeItem> Scopes { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync([FromQuery(Name = "req_id")] String requestId)
        {
            var authorizeQuery = base.HttpContext.Session.GetAuthorizeRequest(requestId);
            if (authorizeQuery is null || User.IsUserAuthenticated() is false)
                return this.RedirectToErrorPage("access_denied", "Request was not validated");

            if (String.IsNullOrEmpty(authorizeQuery.Scope))
                return this.CallbackClientWithError($"{authorizeQuery.RedirectUri}", error: "access_denied", state: authorizeQuery.State);

            var needsConsent = await _consentEvaluationService
                .NeedsConsentAsync(authorizeQuery.Scope.Split(' ').ToList(), authorizeQuery.ClientId, this.GetUserId());
            if (needsConsent is false)
            {
                String grantCode = _grantCodeGenerator.GenerateGrantCode();
                await this.SaveGrantCodeAsync(grantCode, authorizeQuery, this.GetUserId(), CancellationToken.None);

                return this.RedirectToClient(authorizeQuery.RedirectUri!, grantCode, authorizeQuery.State);
            }

            Scopes = authorizeQuery.Scope.Split(' ').Select(s => new ScopeItem { Name = s, IsSelected = true }).ToList();

            base.TempData["req_id"] = requestId;
            return base.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            String? requestId = base.TempData["req_id"]!.ToString();
            if (requestId is null)
                return this.RedirectToErrorPage("server_error", "Illegal state");

            AuthorizeQuery? authorizeQuery = base.HttpContext.Session.GetAuthorizeRequest(requestId);
            if (authorizeQuery is null)
                return this.RedirectToErrorPage("server_error", "Illegal state");

            if (Scopes!.Any(aScope => aScope.IsSelected == false))
                return this.CallbackClientWithError(authorizeQuery.RedirectUri!, error: "access_denied", state: authorizeQuery.State);

            String grantCode = _grantCodeGenerator.GenerateGrantCode();
            await this.SaveGrantCodeAsync(grantCode, authorizeQuery, this.GetUserId(), CancellationToken.None);

            await _upsertUserConsentService.UpsertAsync(Scopes.Select(aScope => aScope.Name).ToList(), authorizeQuery.ClientId, this.GetUserId(), CancellationToken.None);

            return this.RedirectToClient(authorizeQuery.RedirectUri!, grantCode, authorizeQuery.State);
        }

        private async Task SaveGrantCodeAsync(String grantCode, AuthorizeQuery authorizeQuery, String userId, CancellationToken cancellationToken)
        {
            GrantCodeEntity grantCodeEntity = new GrantCodeEntity
            {
                Code = grantCode,
                UserId = userId,
                ClientId = authorizeQuery.ClientId,
                Nonce = authorizeQuery.Nonce,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                CodeChallenge = authorizeQuery.CodeChallenge,
                CodeChallengeMethod = authorizeQuery.CodeChallengeMethod
            };
            await _grantCodeRepository.AddAsync(grantCodeEntity, cancellationToken);
        }

        private IActionResult RedirectToClient(String redirectUri, String grantCode, String? state = null)
        {
            var queryBuilder = new QueryBuilder
            {
                { "code", grantCode }
            };

            if (String.IsNullOrWhiteSpace(state) is false)
                queryBuilder.Add("state", state);

            return base.Redirect($"{redirectUri}{queryBuilder.ToQueryString().Value}");
        }

        private String GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
    }

    public class ScopeItem
    {
        public required String Name { get; set; }
        public Boolean IsSelected { get; set; }
    }
}
