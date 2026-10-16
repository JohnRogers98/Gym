using System.ComponentModel.DataAnnotations;

namespace Gym.Chat.WSApi.Options
{
    public sealed class AuthenticationCookieOptions
    {
        public const String SectionName = "AuthenticationCookie";

        [Required(AllowEmptyStrings = false)]
        public required String Name { get; init; }

        [Required(AllowEmptyStrings = false)]
        public required String DataProtectorPurpose { get; init; }
    }
}
