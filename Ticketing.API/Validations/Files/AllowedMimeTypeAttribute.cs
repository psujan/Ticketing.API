using System.ComponentModel.DataAnnotations;

namespace Ticketing.API.Validations.Files
{
    public class AllowedMimeTypeAttribute:ValidationAttribute
    {
        private static readonly string[] _allowedExtensions = [".jpg" , "jpeg" , ".png" , ".pdf" ,".doc" , ".docx" , ".pdf"];

        

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is List<IFormFile> files)
            {
                foreach (var file in files)
                {
                    var extension = Path.GetExtension(file.FileName).ToLower();
                    if (!_allowedExtensions.Contains(extension))
                    {
                        return new ValidationResult(ErrorMessage ?? $"File type {extension} is not allowed.");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}
