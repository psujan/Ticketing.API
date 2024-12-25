using Ticketing.API.Model.Domain;
using Ticketing.API.Model.Dto.Category;

namespace Ticketing.API.Model.Dto
{
    public class TicketResponseDto:Base<int>
    {
        public string? Title { get; set; }
        public string Status { get; set; } // Active ,  InProgress , Resolved ,  Recreated , 

        public string Details { get; set; }

        //for visitor, they need to provide their email and phone number
        public string? IssuerEmail { get; set; }
        public string? IssuerPhone { get; set; }

        public UserResponseDto User { get; set; }

        public CategoryDto Category { get; set; }

        public ICollection<FileResponseDto>? Files { get; set; }

        //public ICollection<TicketDiscussion>? TicketDiscussions { get; set; }
    }
}
