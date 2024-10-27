using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Ticketing.API.Validations;
using Ticketing.API.Validations.Files;

namespace Ticketing.API.Model.Dto
{
    public class TicketRequestDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [TicketStatus]
        public string Status { get; set; }  // Opened , InProgress, Reopend , Resolved 

        [Required]
        public string Details { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public string? UserId { get; set; }

        [EmailAddress]
        public string? IssuerEmail { get; set; }

        public string? IssuerPhone { get; set; }

        [MaxFileSize (2 * 1024 *1024)]
        [MaxFileCount (3)]
        [AllowedMimeType]
        public List<IFormFile> Files { get; set; }
    }
}
