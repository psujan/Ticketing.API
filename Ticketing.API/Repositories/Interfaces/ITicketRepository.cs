using Ticketing.API.Model.Domain;
using Ticketing.API.Model.Dto.Requuest;

namespace Ticketing.API.Repositories.Interfaces
{
    public interface ITicketRepository : IBaseRepository<Ticket> 
    {
        Task<Ticket> Create(TicketRequestDto ticketRequestDto);
        Task<Ticket ?> Update(int id, TicketRequestDto ticketRequestDto);
        Task<Ticket?> Delete(int id);
        Task<Ticket?> UpdateStatus(int id, string status);
    }
}
