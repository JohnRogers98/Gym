using Ardalis.ApiEndpoints;
using Gym.AuthorizationServer.Entities.Clients;
using Gym.AuthorizationServer.Extensions;
using Gym.AuthorizationServer.Queries;
using Gym.AuthorizationServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.AuthorizationServer.Controllers.Api
{
    [ApiController]
    [AllowAnonymous]
    public class AuthorizeEndpoint(IClientRepository _clientRepository, IRequestIdGenerator _requestIdGenerator, IScopeChecker _scopeChecker) 
        : EndpointBaseAsync.WithRequest<AuthorizeQuery>.WithoutResult
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

            var checkResult = _scopeChecker.CheckScopes(client.ScopesAsString, request.Scope);
            if (checkResult is false)
                return this.CallbackClientWithError(client.RedirectUri, error: "invalid_scope", state: request.State);
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
