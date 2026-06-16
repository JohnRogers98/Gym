using Ardalis.ApiEndpoints;
using Gym.AuthorizationServer.Extensions;
using Gym.AuthorizationServer.Infrastructure.Entities.Clients;
using Gym.AuthorizationServer.Infrastructure.Entities.ProtectedResources;
using Gym.AuthorizationServer.Services;
using Gym.OAuth.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.AuthorizationServer.Controllers.Api
{
    [ApiController]
    [AllowAnonymous]
    public class AuthorizeEndpoint(
        IClientRepository _clientRepository,
        IProtectedResourceRepository _protectedResourceRepository,
        IRequestIdGenerator _requestIdGenerator) : EndpointBaseAsync.WithRequest<AuthorizeQuery>.WithoutResult
    {
        [HttpGet("authorize")]
        public async override Task<IActionResult> HandleAsync(AuthorizeQuery request, CancellationToken cancellationToken = default)
        {
            #region Guard clause
            if (request.ClientId is null)
                return this.RedirectToErrorPage("invalid_request", "Client id is required");

            if (request.RedirectUri is null)
                return this.RedirectToErrorPage("invalid_request", "Redirect uri is required");

            ClientEntity? client = await _clientRepository.GetByIdAsync(request.ClientId, cancellationToken);
            if (client is null)
                return this.RedirectToErrorPage("unknown_client", "Client does not exist");

            if (client.RedirectUri != request.RedirectUri)
                return this.RedirectToErrorPage("invalid_request", "Provided redirect_uri is not registered");

            if(request.Resource is null)
                return this.RedirectToErrorPage("invalid_request", "Resource is requierd");

            var targetProtectedResource = await _protectedResourceRepository.GetByAudienceUriAsync(request.Resource, cancellationToken);
            if (targetProtectedResource is null)
                return this.RedirectToErrorPage("invalid_target", "Provided resource is not registered");

            if (request.CodeChallengeMethod is not null)
            {
                if (request.CodeChallengeMethod != "S256" || request.CodeChallenge is null)
                    return this.CallbackClientWithError("invalid_request", "Unsupported code_challenge_method. Only 'S256' is supported");
            }
            #endregion

            String requestId = _requestIdGenerator.Generate();
            base.HttpContext.Session.SetAuthorizeRequest(requestId, request);
            
            if (this.User.IsUserAuthenticated())
            {
                return base.RedirectToPage("/approve", new { req_id = requestId });   
            }
            else
            {
                return base.RedirectToPage("/login", new { req_id = requestId });
            }
        }

    }
}
