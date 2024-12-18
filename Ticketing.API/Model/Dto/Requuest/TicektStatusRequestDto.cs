using System.ComponentModel.DataAnnotations;
using Ticketing.API.Validations;

namespace Ticketing.API.Model.Dto.Requuest
{
    public class TicektStatusRequestDto
    {
        [Required]
        [TicketStatus]
        public string status { get; set; }
    }
}
