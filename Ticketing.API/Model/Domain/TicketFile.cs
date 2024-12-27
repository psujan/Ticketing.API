using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.API.Model.Domain
{
    public class TicketFile : File
    {
        
        public int TicketId {  get; set; }

        public Ticket Ticket { get; set; }
    }
}
