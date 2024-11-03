using System.ComponentModel.DataAnnotations;
using Ticketing.API.Validations.Files;

namespace Ticketing.API.Model.Dto
{
    public class SolutionGuideRequestDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Details { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string UserId { get; set; } // Added By

        [MaxFileSize(5 * 1024 * 1024)]
        [MaxFileCount(3)]
        [AllowedMimeType]
        public List<IFormFile>? Files { get; set; }
    }
}
