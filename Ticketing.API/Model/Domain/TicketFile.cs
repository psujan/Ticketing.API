using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.API.Model.Domain
{
    public class TicketFile
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TicketId {  get; set; }
        public Ticket Ticket { get; set; }

        public int FileId { get; set; }
        public File File { get; set; }
    }
}
