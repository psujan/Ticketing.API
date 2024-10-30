using Ticketing.API.Model.Domain;
using Ticketing.API.Model.Dto;

namespace Ticketing.API.Repositories.Interfaces
{
    public interface ITicketRepository : IBaseRepository<Ticket> 
    {
        Task<Ticket> Create(TicketRequestDto ticketRequestDto);
        Task<Ticket ?> Update(int id, TicketRequestDto ticketRequestDto);
        Task<Ticket?> Delete(int id);
    }
}
