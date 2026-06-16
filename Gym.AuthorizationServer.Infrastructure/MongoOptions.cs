namespace Gym.AuthorizationServer.Infrastructure
{
    public class MongoOptions
    {
        public String ConnectionString { get; set; } = default!;
        public String DatabaseName { get; set; } = default!;
        public CollectionNames Collections { get; set; } = new();
    }

    public class CollectionNames
    {
        public String Users { get; set; } = default!;
        public String FormCredentials { get; set; } = default!;
        public String TelegramCredentials { get; set; } = default!;
        public String Clients { get; set; } = default!;
        public String UserConsents { get; set; } = default!;
        public String GrantCodes { get; set; } = default!;
        public String AccessTokens { get; set; } = default!;
        public String RefreshTokens { get; set; } = default!;
        public String Roles { get; set; } = default!;
        public String Scopes { get; set; } = default!;
        public String ProtectedResources { get; set; } = default!;
    }
}
