using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Ticketing.API.Validations
{
    public class RequiredIfAttribute:ValidationAttribute
    {
        private readonly string _dependentProperty;
        private readonly object _targetValue;

        public RequiredIfAttribute(string dependentProperty, object targetValue)
        {
            _dependentProperty = dependentProperty;
            _targetValue = targetValue;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Get the dependent property value
            var dependentPropertyInfo = validationContext.ObjectType.GetProperty(_dependentProperty, BindingFlags.Public | BindingFlags.Instance);
            if (dependentPropertyInfo == null)
            {
                return new ValidationResult($"Property {_dependentProperty} not found.");
            }

            var dependentValue = dependentPropertyInfo.GetValue(validationContext.ObjectInstance);

            // Check if the dependent property's value matches the target value
            if ((dependentValue == null && _targetValue == null) || (dependentValue != null && dependentValue.Equals(_targetValue)))
            {
                // Validate the current property value
                if (value == null || (value is string stringValue && string.IsNullOrWhiteSpace(stringValue)))
                {
                    return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} is required.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
