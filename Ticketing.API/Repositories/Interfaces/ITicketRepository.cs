using Ticketing.API.Model;
using Ticketing.API.Model.Domain;
using Ticketing.API.Model.Dto;
using Ticketing.API.Model.Dto.Requuest;
using TicketResponseDto = Ticketing.API.Model.Dto.TicketResponseDto;

namespace Ticketing.API.Repositories.Interfaces
{
    public interface ITicketRepository : IBaseRepository<TicketResponseDto> 
    {
        Task<TicketResponseDto> Create(TicketRequestDto ticketRequestDto);
        Task<TicketResponseDto?> Update(int id, TicketRequestDto ticketRequestDto);
        Task<TicketResponseDto?> Delete(int id);
        Task<TicketResponseDto?> UpdateStatus(int id, string status);
        new Task<TicketResponseDto> GetById(int id);

        Task<bool> DeleteTicketFile(int fileId);

        Task<PaginatedModel<TicketResponseDto>> GetUserTickets(string userName , int pageNo , int pageSize);
    }
}
