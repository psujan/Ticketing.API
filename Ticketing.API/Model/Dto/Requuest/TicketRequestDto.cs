using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Ticketing.API.Validations;
using Ticketing.API.Validations.Files;

namespace Ticketing.API.Model.Dto.Requuest
{
    public class TicketRequestDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [TicketStatus]
        public string Status { get; set; }  

        [Required]
        public string Details { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public string? UserId { get; set; }

        [RequiredIf("UserId" , null , ErrorMessage ="Email is required")]
        [EmailAddress]
        public string? IssuerEmail { get; set; }

        [RequiredIf("UserId", null, ErrorMessage = "Phone number is required")]
        public string? IssuerPhone { get; set; }

        [MaxFileSize(2 * 1024 * 1024)]
        [MaxFileCount(3)]
        [AllowedMimeType]
        public List<IFormFile>? Files { get; set; }
    }
}
