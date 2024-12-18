using System.ComponentModel.DataAnnotations;

namespace Ticketing.API.Model.Dto.Requuest
{
    public class TicketDiscussionRequestDto
    {
        [Required]
        public string Comment { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedAt { get; set; }

        [Required]
        public int TicketId { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
