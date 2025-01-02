using Microsoft.AspNetCore.Identity;

namespace Ticketing.API.Model.Domain
{
    public class User :IdentityUser
    {
        public string? FullName { get; set; }

        public string? Address { get; set; }

        public Boolean Status { get; set; } = true;

        // IdentityUser already includes Id property
        public virtual ICollection<SolutionGuide> SolutionGuide { get; set; }

        public virtual ICollection<TicketDiscussion> TicketDiscussion { get; set; }

    }
}
