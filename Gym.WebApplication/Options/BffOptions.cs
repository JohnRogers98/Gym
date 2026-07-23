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
        [Required] public String ListAvailableClientCalendarEventsEndpoint { get; set; } = default!;
        [Required] public String BookCalendarEventEndpoint { get; set; } = default!;
        [Required] public String ListSessionClientCalendarEventsEndpoint { get; set; } = default!;
        [Required] public String ListSessionClientPersonalTrainingsEndpoint { get; set; } = default!;

        [Required] public String ListClientsForAdminEndpoint { get; set; } = default!;
        [Required] public String ChargeClientEndpoint { get; set; } = default!;
        [Required] public String CreateCalendarEventEndpoint { get; set; } = default!;
        [Required] public String CancelCalendarEventEndpoint { get; set; } = default!;
        [Required] public String CreateTrainingEndpoint { get; set; } = default!;

        [Required] public String ListInstructorsEndpoint { get; set; } = default!;
        [Required] public String ListTrainingsEndpoint { get; set; } = default!;
        [Required] public String ListCalendarEventsForAdminEndpoint { get; set; } = default!;
    }

    public static class UrlHelper
    {
        public static string ReplacePathVariables(String template, Dictionary<String, String> parameters)
        {
            var result = template;
            foreach (var param in parameters)
            {
                if (!template.Contains($"{{{param.Key}}}"))
                {
                    throw new ArgumentException($"Parameter '{param.Key}' not found in template");
                }
                result = result.Replace($"{{{param.Key}}}", param.Value);
            }
            return result;
        }
    }
}
