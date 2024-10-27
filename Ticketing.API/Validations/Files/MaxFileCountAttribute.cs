using System.ComponentModel.DataAnnotations;

namespace Ticketing.API.Validations.Files
{
    public class MaxFileCountAttribute:ValidationAttribute
    {
        private readonly int maxCount;

        public MaxFileCountAttribute(int c)
        {
            maxCount = c;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is List<IFormFile> files && files.Count > maxCount)
            {
                return new ValidationResult(ErrorMessage ?? $"You can upload up to {maxCount} files.");
            }

            return ValidationResult.Success;
        }
    }
}
