
namespace Gym.OAuth.Extensions;

public static class AuthorizeQueryExtensions
{
    extension (AuthorizeQuery query)
    {
        public String ToQueryString()
        {
            var parameters = new List<String>();

            if (!String.IsNullOrEmpty(query.ClientId))
                parameters.Add($"client_id={Uri.EscapeDataString(query.ClientId)}");

            if (!String.IsNullOrEmpty(query.ResponseType))
                parameters.Add($"response_type={Uri.EscapeDataString(query.ResponseType)}");

            if (!String.IsNullOrEmpty(query.RedirectUri))
                parameters.Add($"redirect_uri={Uri.EscapeDataString(query.RedirectUri)}");

            if (!String.IsNullOrEmpty(query.Scope))
                parameters.Add($"scope={Uri.EscapeDataString(query.Scope)}");

            if (!String.IsNullOrEmpty(query.State))
                parameters.Add($"state={Uri.EscapeDataString(query.State)}");

            if (!String.IsNullOrEmpty(query.CodeChallenge))
                parameters.Add($"code_challenge={Uri.EscapeDataString(query.CodeChallenge)}");

            if (!String.IsNullOrEmpty(query.CodeChallengeMethod))
                parameters.Add($"code_challenge_method={Uri.EscapeDataString(query.CodeChallengeMethod)}");

            if (!String.IsNullOrEmpty(query.Nonce))
                parameters.Add($"nonce={Uri.EscapeDataString(query.Nonce)}");

            return parameters.Any() ? $"?{String.Join("&", parameters)}" : String.Empty;
        }
    }
 
}
