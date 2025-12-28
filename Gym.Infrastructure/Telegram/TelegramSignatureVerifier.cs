using Gym.Domain;
using Gym.Domain.Users;
using Gym.Domain.Users.Authentication;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Web;
using Telegram.Bot.Types;

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
                User tgUser = webAppInitData.GetUser();
                return Result<ValidatedTelegramUserInfo>.Ok(ValidatedTelegramUserInfo.From(TelegramUserId.From(tgUser.Id)));
            }
            else
            {
                return Result<ValidatedTelegramUserInfo>.Fail("Hash is not valid");
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

        public User GetUser()
        {
            String userJson = _parsedTgWebAppData["user"]!;

            using JsonDocument userJsonDoc = JsonDocument.Parse(userJson);
            var root = userJsonDoc.RootElement;

            return new User
            {
                Id = root.GetProperty("id").GetInt64(),
                FirstName = root.GetProperty("first_name").GetString() ?? "",
                LastName = root.GetProperty("last_name").GetString(),
                Username = root.GetProperty("username").GetString(),
            };
        }
    }

}
