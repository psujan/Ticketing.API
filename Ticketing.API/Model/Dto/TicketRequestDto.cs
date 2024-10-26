using Microsoft.AspNetCore.Identity;

namespace Ticketing.API.Model.Dto
{
    public class TicketRequestDto
    {
        public string? Title { get; set; }
        public string Status { get; set; }

        public string Details { get; set; }

        public int CategoryId { get; set; }

        public Guid? UserId { get; set; }

        public List<IFormFile> Files { get; set; }
    }
}
