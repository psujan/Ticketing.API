using System.ComponentModel.DataAnnotations;

namespace Ticketing.API.Validations
{
    public class TicketStatusAttribute:ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string[] validStatus = ["Active", "InProgress", "Reopened", "Resolved", "Terminated"];

            if (validStatus.Contains(value))
            {
                return true;
            }

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} is not valid string";
        }
    }
}
