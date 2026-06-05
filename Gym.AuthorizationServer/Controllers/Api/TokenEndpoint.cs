using Ardalis.ApiEndpoints;
using Gym.AuthorizationServer.Extensions;
using Gym.AuthorizationServer.Infrastructure.Entities.Clients;
using Gym.AuthorizationServer.Services;
using Gym.AuthorizationServer.Services.Flows;
using Gym.OAuth.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Gym.AuthorizationServer.Controllers.Api
{
    [Route("token")]
    [ApiController]
    public class TokenEndpoint(
        IClientRepository _clientRepository,
        IClientSecretHashValidator _clientSecretHashValidator,
        ITokenFlowCoordinator _tokenFlowCoordinator)
        : EndpointBaseAsync.WithRequest<TokenRequest>.WithActionResult<TokenResponse>
    {
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        [Produces("application/json")]
        public override async Task<ActionResult<TokenResponse>> HandleAsync(TokenRequest request, CancellationToken cancellationToken = default)
        {
            ClientEntity? client;

            var headerBasicAuth = base.HttpContext.Request.Headers.GetCredentialsFromBasicAuthorization();
            if (headerBasicAuth is not null)
            {
                if (String.IsNullOrWhiteSpace(request.ClientId) is false || String.IsNullOrWhiteSpace(request.ClientSecret) is false)
                    return base.BadRequest(new { Error = "invalid_request", ErrorDesctiption = "Duplicated client credentials" });

                client = await _clientRepository.GetByIdAsync(headerBasicAuth.Value.login, cancellationToken);
                if (client is null || _clientSecretHashValidator.ValidateSecret(client.SecretHash, headerBasicAuth.Value.password) is false)
                {
                    HttpContext.Response.Headers.Append("WWW-Authenticate", "Basic realm=\"OAuth\"");
                    return base.Unauthorized(new { Error = "invalid_client", ErrorDescription = "No such client exist" });
                }
            }
            else
            {
                if (String.IsNullOrWhiteSpace(request.ClientId) || String.IsNullOrWhiteSpace(request.ClientSecret))
                    return base.BadRequest(new { Error = "invalid_client", ErrorDescription = "No client credentials" });

                client = await _clientRepository.GetByIdAsync(request.ClientId, cancellationToken);
                if (client is null || _clientSecretHashValidator.ValidateSecret(client.SecretHash, request.ClientSecret) is false)
                    return base.BadRequest(new { Error = "invalid_client", ErrorDescription = "No such client exist" });
            }

            if (client.RedirectUri != request.RedirectUri)
                return base.BadRequest(new { Error = "invalid_client", ErrorDescription = "redirect_uri mismathced" });

            if (String.IsNullOrWhiteSpace(request.GrantType))
                return base.BadRequest(new { Error = "invalid_request", ErrorDescription = "No grant type presented" });

            switch (request.GrantType)
            {
                case GrantTypes.AuthorizationCode:
                    {
                        if (String.IsNullOrWhiteSpace(request.Code))
                            return base.BadRequest(new { Error = "invalid_request" });

                        AuthorizationCodeRequest authorizationCodeRequest = new()
                        {
                            ClientId = client.Id,
                            Code = request.Code,
                            CodeVerifier = request.CodeVerifier
                        }; 

                        var authorizationCodeFlowResult = await _tokenFlowCoordinator.AuthorizationCodeAsync(authorizationCodeRequest, cancellationToken);
                        if(authorizationCodeFlowResult.IsFailed)
                            return base.BadRequest(new { Error = authorizationCodeFlowResult.ErrorCode, authorizationCodeFlowResult.ErrorDescription });

                        TokenResponse tokenResponse = new()
                        {
                            AccessToken = authorizationCodeFlowResult.Value.AccessToken,
                            RefreshToken = authorizationCodeFlowResult.Value.RefreshToken,
                            TokenType = authorizationCodeFlowResult.Value.TokenType,
                            ExpiresIn = authorizationCodeFlowResult.Value.ExpiresIn,
                            Scope = authorizationCodeFlowResult.Value.Scope,
                            IdToken = authorizationCodeFlowResult.Value.IdToken
                        };

                        return base.Ok(tokenResponse);
                    }
                case GrantTypes.RefreshToken:
                    {
                        if (String.IsNullOrWhiteSpace(request.RefreshToken))
                            return base.BadRequest(new { Error = "invalid_request" });

                        var refreshTokenFlowResult = await _tokenFlowCoordinator.RefreshTokenAsync(new RefreshTokenRequest { RefreshToken = request.RefreshToken }, cancellationToken);
                        if (refreshTokenFlowResult.IsFailed)
                            return base.BadRequest(new { Error = refreshTokenFlowResult.ErrorCode, refreshTokenFlowResult.ErrorDescription });

                        TokenResponse tokenResponse = new()
                        {
                            AccessToken = refreshTokenFlowResult.Value.AccessToken,
                            RefreshToken = refreshTokenFlowResult.Value.RefreshToken,
                            TokenType = refreshTokenFlowResult.Value.TokenType,
                            ExpiresIn = refreshTokenFlowResult.Value.ExpiresIn,
                            Scope = refreshTokenFlowResult.Value.Scope,
                            IdToken = refreshTokenFlowResult.Value.IdToken
                        };

                        return base.Ok(tokenResponse);
                    }
                case GrantTypes.TelegramAssertion:
                    {
                        if (String.IsNullOrWhiteSpace(request.Assertion))
                            return base.BadRequest(new { Error = "invalid_request" });

                        if (String.IsNullOrWhiteSpace(request.Scope))
                            return base.BadRequest(new { Error = "invalid_request" });


                        TelegramAssertionRequest telegramAssertionRequest = new()
                        {
                            Assertion = request.Assertion,
                            ClientId = client.Id,
                            Scope = request.Scope
                        };

                        var telegramAssertionFlowResult = await _tokenFlowCoordinator.TelegramAssertionAsync(telegramAssertionRequest, cancellationToken);
                        if (telegramAssertionFlowResult.IsFailed)
                            return base.BadRequest(new { Error = telegramAssertionFlowResult.ErrorCode, telegramAssertionFlowResult.ErrorDescription });

                        TokenResponse tokenResponse = new()
                        {
                            AccessToken = telegramAssertionFlowResult.Value.AccessToken,
                            RefreshToken = telegramAssertionFlowResult.Value.RefreshToken,
                            TokenType = telegramAssertionFlowResult.Value.TokenType,
                            ExpiresIn = telegramAssertionFlowResult.Value.ExpiresIn,
                            Scope = telegramAssertionFlowResult.Value.Scope,
                            IdToken = telegramAssertionFlowResult.Value.IdToken
                        };

                        return base.Ok(tokenResponse);
                    }

                default:
                    {
                        return base.BadRequest(new { Error = "unsupported_grant_type" });
                    }
            }
        }

    }
}
