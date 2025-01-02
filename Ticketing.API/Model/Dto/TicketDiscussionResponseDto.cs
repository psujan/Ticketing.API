using Ticketing.API.Model.Domain;

namespace Ticketing.API.Model.Dto
{
    public class TicketDiscussionResponseDto
    {
        public int Id { get; set; }
        public string Comment { get; set; }

        public DateTime? DeletedAt { get; set; }

        public int TicketId { get; set; }

        public string UserId { get; set; }

        public UserResponseDto User { get; set; }
    }
}
