namespace Gym.AuthorizationServer.Claims
{
    public class IdToken
    {
        public required String Issuer {  get; set; }
        public required String Subject {  get; set; }
        public required String Audience {  get; set; }

        public required Int64 Expiration { get; set; }
        public required Int64 IssuedAt { get; set; }

        public Int64? AuthenticationTime { get; set; }
        public String? Nonce { get; set; }

        public String? AuthenticationContextClassReference { get; set; }
        public List<String>? AuthenticationMethodsReferences { get; set; }

        public String? AtHash { get; set; }
    }
}
