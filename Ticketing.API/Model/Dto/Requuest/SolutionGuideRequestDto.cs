using System.ComponentModel.DataAnnotations;
using Ticketing.API.Validations.Files;

namespace Ticketing.API.Model.Dto.Requuest
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
        public string UserName { get; set; } // Added By

        [MaxFileSize(5 * 1024 * 1024)]
        [MaxFileCount(3)]
        [AllowedMimeType]
        public List<IFormFile>? Files { get; set; }
    }
}
