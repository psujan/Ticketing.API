using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.API.Model.Domain
{
    public class TicketDiscussion : Base<int>
    {
        public string Comment { get; set; }

        public DateTime? DeletedAt {  get; set; }

        public int TicketId { get; set; }

        public string UserId { get; set; }

        public Ticket Ticket { get; set; }

        public User User { get; set; }

    }
}
