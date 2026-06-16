using Gym.AuthorizationServer.Extensions;
using Gym.AuthorizationServer.Infrastructure.Entities.GrantCodes;
using Gym.AuthorizationServer.Infrastructure.Entities.ProtectedResources;
using Gym.AuthorizationServer.Infrastructure.Entities.UserConsents;
using Gym.AuthorizationServer.Infrastructure.Entities.Users;
using Gym.AuthorizationServer.Services;
using Gym.OAuth.Extensions;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Gym.AuthorizationServer.Pages
{
    public class ApproveModel(
        IUserRepository _userRepository,
        IProtectedResourceRepository _protectedResourceRepository,
        IScopeGrantResolveService _scopeGrantResolveService,
        IGrantCodeGenerator _grantCodeGenerator,
        IGrantCodeRepository _grantCodeRepository,
        IConsentEvaluationService _consentEvaluationService,
        IUpsertUserConsentService _upsertUserConsentService) : PageModel
    {
        [BindProperty]
        public List<ScopeItem> Scopes { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync([FromQuery(Name = "req_id")] String requestId, CancellationToken cancellationToken)
        {
            var authorizeQuery = base.HttpContext.Session.GetAuthorizeRequest(requestId);
            if (authorizeQuery is null || User.IsUserAuthenticated() is false)
                return this.RedirectToErrorPage("access_denied", "Request was not validated");

            var user = await _userRepository.GetByIdAsync(this.GetUserId(), cancellationToken);
            var targetProtectedResource = await _protectedResourceRepository.GetByAudienceUriAsync(authorizeQuery.Resource!, cancellationToken);

            var scopeResolveResult = await _scopeGrantResolveService.Resolve(user!.RoleId, targetProtectedResource!.Id, authorizeQuery.Scope, cancellationToken);
            if (scopeResolveResult.IsFailed)
                return this.CallbackClientWithError(authorizeQuery.RedirectUri!, error: "invalid_scope", state: authorizeQuery.State);

            Boolean needConsent = await _consentEvaluationService
                   .NeedsConsentAsync(scopeResolveResult.Value, this.GetUserId(), authorizeQuery.ClientId, targetProtectedResource.Id, cancellationToken);
            if (needConsent is false)
            {
                String grantCode = _grantCodeGenerator.GenerateGrantCode();
                await this.SaveGrantCodeAsync(grantCode, authorizeQuery, this.GetUserId(), targetProtectedResource!.Id, cancellationToken);

                return this.RedirectToClient(authorizeQuery.RedirectUri!, grantCode, authorizeQuery.State);
            }

            Scopes = scopeResolveResult.Value.Select(s => new ScopeItem { Id = s.Id, Name = s.Name, IsSelected = true }).ToList();

            base.TempData["req_id"] = requestId;
            base.TempData["target_protected_resource_id"] = targetProtectedResource!.Id;
            return base.Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            #region Guard clause
            String? requestId = base.TempData["req_id"]!.ToString();
            if (requestId is null)
                return this.RedirectToErrorPage("server_error", "Illegal state");

            String? targetProtectedResourceId = base.TempData["target_protected_resource_id"]!.ToString();
            if (targetProtectedResourceId is null)
                return this.RedirectToErrorPage("server_error", "Illegal state");

            AuthorizeQuery? authorizeQuery = base.HttpContext.Session.GetAuthorizeRequest(requestId);
            if (authorizeQuery is null)
                return this.RedirectToErrorPage("server_error", "Illegal state");

            if (Scopes!.Any(aScope => aScope.IsSelected == false))
                return this.CallbackClientWithError(authorizeQuery.RedirectUri!, error: "access_denied", state: authorizeQuery.State);
            #endregion

            String grantCode = _grantCodeGenerator.GenerateGrantCode();
            await this.SaveGrantCodeAsync(grantCode, authorizeQuery, this.GetUserId(), targetProtectedResourceId, cancellationToken);

            await _upsertUserConsentService.UpsertAsync(
                Scopes.Select(aScope => new ScopeInfo() { Id = aScope.Id, Name = aScope.Name } ),
                this.GetUserId(),
                authorizeQuery.ClientId,
                targetProtectedResourceId,
                cancellationToken);

            return this.RedirectToClient(authorizeQuery.RedirectUri!, grantCode, authorizeQuery.State);
        }

        private async Task SaveGrantCodeAsync(
            String grantCode,
            AuthorizeQuery authorizeQuery,
            String userId,
            String protectedResourceId,
            CancellationToken cancellationToken)
        {
            GrantCodeEntity grantCodeEntity = new GrantCodeEntity
            {
                Code = grantCode,
                UserId = userId,
                ClientId = authorizeQuery.ClientId,
                ProtectedResourceId = protectedResourceId,
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
        public required String Id { get; set; }
        public required String Name { get; set; }
        public Boolean IsSelected { get; set; }
    }
}
