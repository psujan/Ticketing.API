using System.ComponentModel.DataAnnotations;

namespace Ticketing.API.Model.Dto
{
    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
