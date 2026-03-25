using System.ComponentModel.DataAnnotations;

namespace Gym.WebApplication.Features.Admin.CalendarEvents.Creation.Models.Forms
{
    public class MinItemsAttribute : ValidationAttribute
    {
        private readonly Int32 _minItems;

        public MinItemsAttribute(Int32 minItems)
        {
            _minItems = minItems;
            ErrorMessage = $"At least {minItems} item(s) are required";
        }

        protected override ValidationResult? IsValid(Object? value, ValidationContext validationContext)
        {
            if (value is null)
                return new ValidationResult(ErrorMessage);

            if (value is System.Collections.IList list && list.Count >= _minItems)
            {
                return ValidationResult.Success;
            }       

            //return new ValidationResult(ErrorMessage);
            return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName! });
        }
    }
}
