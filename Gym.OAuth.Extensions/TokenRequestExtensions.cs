namespace Gym.OAuth.Extensions;

public static class TokenRequestExtensions
{
    extension (TokenRequest request)
    {
        public FormUrlEncodedContent ToFormContent()
        {
            var formData = new Dictionary<String, String>();

            if (!String.IsNullOrEmpty(request.ClientId))
                formData["client_id"] = request.ClientId;
            if (!String.IsNullOrEmpty(request.ClientSecret))
                formData["client_secret"] = request.ClientSecret;
            if (!String.IsNullOrEmpty(request.RedirectUri))
                formData["redirect_uri"] = request.RedirectUri;
            if (!String.IsNullOrEmpty(request.GrantType))
                formData["grant_type"] = request.GrantType;
            if (!String.IsNullOrEmpty(request.Code))
                formData["code"] = request.Code;
            if (!String.IsNullOrEmpty(request.Scope))
                formData["scope"] = request.Scope;
            if (!String.IsNullOrEmpty(request.RefreshToken))
                formData["refresh_token"] = request.RefreshToken;
            if (!String.IsNullOrEmpty(request.Assertion))
                formData["assertion"] = request.Assertion;
            if (!String.IsNullOrEmpty(request.CodeVerifier))
                formData["code_verifier"] = request.CodeVerifier;

            return new FormUrlEncodedContent(formData);
        }
    }
}
