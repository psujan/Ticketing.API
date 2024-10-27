using System.ComponentModel.DataAnnotations;

namespace Ticketing.API.Validations.Files
{
    public class MaxFileSizeAttribute:ValidationAttribute
    {
        private readonly int maxSizeInBytes;

        public MaxFileSizeAttribute()
        {
            maxSizeInBytes = 5000000; // roughly 5 MB as default
        }

        public MaxFileSizeAttribute(int maxSizeInBytes)
        {
            this.maxSizeInBytes = maxSizeInBytes;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is List<IFormFile> files)
            {
                int i = 0;
                foreach (var file in files)
                {
                    decimal sizeInMb = file.Length / 1024 / 1024;
                    i++;
                    if (file.Length > maxSizeInBytes)
                    {
                        return new ValidationResult(ErrorMessage ?? $"File size cannot exceed {maxSizeInBytes / 1024 / 1024} MB. File named {file.FileName} is {sizeInMb} Mb");
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
