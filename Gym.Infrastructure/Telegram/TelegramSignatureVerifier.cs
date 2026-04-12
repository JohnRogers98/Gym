using Gym.Application.Extensions;
using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.TelegramAuthContext;
using Gym.Domain.TelegramAuthContext.Errors;
using Gym.Domain.TelegramAuthContext.ValueObjects;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Web;
using static Gym.Infrastructure.Telegram.WebAppInitData;

namespace Gym.Infrastructure.Telegram
{
    internal class TelegramSignatureVerifier(TelegramBotToken _botToken) : ITelegramSignatureVerifier
    {
        public Result<ValidatedTelegramUserInfo> Verify(String rawInitData)
        {
            WebAppInitData webAppInitData = WebAppInitData.FromRawUrlQueryString(rawInitData);

            String dataCheckString = webAppInitData.GetDataCheckString();

            String computedHash = this.ComputeHexValidationHash(dataCheckString);

            if(computedHash == webAppInitData.GetHash())
            {
                TelegramUser tgUser = webAppInitData.GetUser();
                
                return Result<ValidatedTelegramUserInfo>.Ok(
                    ValidatedTelegramUserInfo.From(
                        TelegramId.From(tgUser.Id).Unwrap(),
                        String.IsNullOrWhiteSpace(tgUser.Username) ? null : TelegramUsername.From(tgUser.Username).Unwrap(),
                        tgUser.FirstName is not null ? FirstName.From(tgUser.FirstName).Unwrap() : null,
                        String.IsNullOrWhiteSpace(tgUser.LastName) ? null : LastName.From(tgUser.LastName).Unwrap()
                    )
                );
            }
            else
            {
                return Result<ValidatedTelegramUserInfo>.Fail(TelegramInitDataInvalidHashError.Create());
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
    }

    internal class WebAppInitData
    {
        private NameValueCollection _parsedInitData;
        private NameValueCollection _parsedTgWebAppData;

        public WebAppInitData(String rawInitData)
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

        public class TelegramUser
        {
            public Int64 Id { get; set; }
            public String? FirstName { get; set; }
            public String? LastName { get; set; }
            public String? Username { get; set; }
        }

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
