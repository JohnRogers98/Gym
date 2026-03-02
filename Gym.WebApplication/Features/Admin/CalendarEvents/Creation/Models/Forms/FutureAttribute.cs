using System.ComponentModel.DataAnnotations;

namespace Gym.WebApplication.Features.Admin.CalendarEvents.Creation.Models.Forms
{
    public class FutureAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(Object? value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return ValidationResult.Success;
            }

            if (value is DateTime dateTime && dateTime > DateTime.UtcNow)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult($"{validationContext.DisplayName} must be future.");
        }
    }
}
