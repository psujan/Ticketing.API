using Microsoft.AspNetCore.Identity;

namespace Ticketing.API.Model.Domain
{
    public class User :IdentityUser
    {
        public string? FirstName;

        public string? LastName;

        public string? Address;

        public Boolean Status { get; set; } = true;

        // IdentityUser already includes Id property
        public virtual ICollection<SolutionGuide> SolutionGuide { get; set; }

        public virtual ICollection<TicketDiscussion> TicketDiscussion { get; set; }

    }
}
