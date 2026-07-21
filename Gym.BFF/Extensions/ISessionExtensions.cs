namespace Microsoft.AspNetCore.Http;

public static class ISessionExtensions
{
    extension(ISession session)
    {
        public void SetOAuthState(String state) => session.SetString("oauth_state", state);
        public String? ConsumeOAuthState()
        {
            var value = session.GetString("oauth_state");
            session.Remove("oauth_state");
            return value;
        }

        public void SetOAuthNonce(String nonce) => session.SetString("oauth_nonce", nonce);
        public String? ConsumeOAuthNonce()
        {
            var value = session.GetString("oauth_nonce");
            session.Remove("oauth_nonce");
            return value;
        }

        public void SetOAuthCodeVerifier(String codeVerifier) => session.SetString("oauth_code_verifier", codeVerifier);
        public String? ConsumeOAuthCodeVerifier() 
        {
            var value = session.GetString("oauth_code_verifier");
            session.Remove("oauth_code_verifier");
            return value;
        }
    }

}