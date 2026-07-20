namespace Gym.AuthorizationServer.Infrastructure
{
    public class MongoOptions
    {
        public String ConnectionString { get; set; } = "mongodb://localhost:27017/?replicaSet=rs0";
        public String DatabaseName { get; set; } = "auth-server";
        public CollectionNames Collections { get; set; } = new();
    }

    public class CollectionNames
    {
        public String Users { get; set; } = "users";
        public String FormCredentials { get; set; } = "form-credentials";
        public String TelegramCredentials { get; set; } = "telegram-credentials";
        public String Clients { get; set; } = "clients";
        public String UserConsents { get; set; } = "user-consents";
        public String GrantCodes { get; set; } = "grant-codes";
        public String AccessTokens { get; set; } = "access-tokens";
        public String RefreshTokens { get; set; } = "refresh-tokens";
        public String Roles { get; set; } = "roles";
        public String Scopes { get; set; } = "scopes";
        public String ProtectedResources { get; set; } = "protected-resources";
    }
}
