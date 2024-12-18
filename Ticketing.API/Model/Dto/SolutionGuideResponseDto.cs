using System.ComponentModel.DataAnnotations;
using Ticketing.API.Model.Domain;
using Ticketing.API.Validations.Files;

namespace Ticketing.API.Model.Dto
{
    public class SolutionGuideResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Details { get; set; }

        public string Status { get; set; }

       // public DateTime CreatedAt { get; set; }

       // public DateTime UpdatedAt { get; set; }

        public UserResponseDto User { get; set; }

        //public Domain.File Files { get; set; }

        
       public List<FileResponseDto>? Files { get; set; }
    }
}
