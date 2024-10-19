using System.ComponentModel.DataAnnotations;

namespace Ticketing.API.Model.Dto.Category
{
    public class CategoryRequestDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public bool Status { get; set; }

    }
}
