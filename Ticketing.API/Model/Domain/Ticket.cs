using Microsoft.AspNetCore.Identity;

namespace Ticketing.API.Model.Domain
{
    public class Ticket :Base<int>
    {
        public string?  Title { get; set; }
        public string Status { get; set; }

        public string Details { get; set; }
        
        //for visitor, they need to provide their email and phone number
        public string? IssuerEmail { get; set; }
        public string? IssuerPhone { get; set; }

        public string? UserId { get; set; }
        public IdentityUser? User { get; set; } 
        
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<TicketFile>? TicketFiles { get; set; }

    }
}
