using Gym.AuthorizationServer.Shared.Abstractions;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Web;

namespace Gym.AuthorizationServer.Services
{
    public record TelegramBotToken(String Value);

    public class TelegramUser
    {
        public Int64 Id { get; set; }
        public String? FirstName { get; set; }
        public String? LastName { get; set; }
        public String? Username { get; set; }
    }

    public interface ITelegramSignatureVerifier
    {
        Result<TelegramUser> Verify(String rawInitData);
    }

    public class TelegramSignatureVerifier(TelegramBotToken _botToken) : ITelegramSignatureVerifier
    {
        public Result<TelegramUser> Verify(String rawInitData)
        {
            WebAppInitData webAppInitData = WebAppInitData.FromRawUrlQueryString(rawInitData);

            String dataCheckString = webAppInitData.GetDataCheckString();

            String computedHash = this.ComputeHexValidationHash(dataCheckString);

            if (computedHash == webAppInitData.GetHash())
            {
                return Result<TelegramUser>.Success(webAppInitData.GetUser());
            }
            else
            {
                return Result<TelegramUser>.Failure("Hash not valid.");
            }
        }

        private String ComputeHexValidationHash(String dataCheckString)
        {
            Byte[] validationHash = HMACSHA256.HashData(this.GetHMACSecretKey(), Encoding.UTF8.GetBytes(dataCheckString));

            return BitConverter.ToString(validationHash)
                    .Replace("-", "")
                    .ToLowerInvariant();
        }

        private Byte[] GetHMACSecretKey()
            => HMACSHA256.HashData(Encoding.UTF8.GetBytes("WebAppData"), Encoding.UTF8.GetBytes(_botToken.Value));


        private class WebAppInitData
        {
            private NameValueCollection _parsedInitData;
            private NameValueCollection _parsedTgWebAppData;

            private WebAppInitData(String rawInitData)
            {
                _parsedInitData = HttpUtility.ParseQueryString(rawInitData);
                _parsedTgWebAppData = HttpUtility.ParseQueryString(_parsedInitData["tgWebAppData"]!);
            }

            public static WebAppInitData FromRawUrlQueryString(String rawInitData) => new(rawInitData);

            public String GetDataCheckString()
            {
                var sortedKeysWithRemovedHash = _parsedTgWebAppData.AllKeys
                    .Where(key => key != "hash")
                    .OrderBy(key => key);

                return String.Join('\n',
                    sortedKeysWithRemovedHash.Select(key => $"{key}={_parsedTgWebAppData[key] ?? ""}"));
            }

            public String GetHash() => _parsedTgWebAppData["hash"]!;

            public TelegramUser GetUser()
            {
                String userJson = _parsedTgWebAppData["user"]!;

                using JsonDocument userJsonDoc = JsonDocument.Parse(userJson);
                var root = userJsonDoc.RootElement;

                return new TelegramUser
                {
                    Id = root.GetProperty("id").GetInt64(),
                    FirstName = root.GetProperty("first_name").GetString(),
                    LastName = root.TryGetProperty("last_name", out var lastName) is true ? lastName.GetString() : null,
                    Username = root.TryGetProperty("username", out var username) is true ? username.GetString() : null,
                };
            }
        }

    }
}
