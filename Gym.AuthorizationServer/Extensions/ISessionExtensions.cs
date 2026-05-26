using Gym.AuthorizationServer.Queries;
using System.Text.Json;

namespace Gym.AuthorizationServer.Extensions
{
    public static class ISessionExtensions
    {
        public static void SetAuthorizeRequest(this ISession session, String requestId, AuthorizeQuery authorizeQuery)
            => session.SetString($"oauth_{requestId}", JsonSerializer.Serialize(authorizeQuery));

        public static Boolean ContainsAuthorizeRequest(this ISession session, String requestId)
            => session.Keys.Any(key => key == $"oauth_{requestId}");

        public static AuthorizeQuery? GetAuthorizeRequest(this ISession session, String requestId)
        {
            return GetFromJson<AuthorizeQuery>(session, $"oauth_{requestId}");
        }

        public static T? GetFromJson<T>(this ISession session, String key)
        {
            String? json = session.GetString(key);
            return json is null ? default : JsonSerializer.Deserialize<T>(json);
        }
    }
}