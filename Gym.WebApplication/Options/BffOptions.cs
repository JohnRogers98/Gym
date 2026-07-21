using System.ComponentModel.DataAnnotations;

namespace Gym.WebApplication.Options
{
    public class BffOptions
    {
        public const String SectionName = "Bff";

        [Required]
        [Url]
        public String BaseUrl { get; set; } = default!;

        [Required] public String ClientName { get; set; } = default!;

        [Required] public String LoginEndpoint { get; set; } = default!;

        [Required] public String TelegramInitEndpoint { get; set; } = default!;

        [Required] public String CheckSessionEndpoint { get; set; } = default!;

        [Required] public String SessionInfoEndpoint { get; set; } = default!;

        [Required] public String LogoutEndpoint { get; set; } = default!;

        [Required] public String ListRolesEndpoint { get; set; } = default!;
        [Required] public String CreateUserEndpoint { get; set; } = default!;
        [Required] public String CheckUsernameEndpoint { get; set; } = default!;
        [Required] public String ChangePasswordEndpoint { get; set; } = default!;


        [Required] public String GetClientDetailsEndpoint { get; set; } = default!;
        [Required] public String GetAccountHistoryEndpoint { get; set; } = default!;
    }
}
