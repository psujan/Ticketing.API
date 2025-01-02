using Ticketing.API.Model.Domain;
using Ticketing.API.Model.Dto;
using Ticketing.API.Model.Dto.Category;
using Ticketing.API.Model.Dto.Requuest;

namespace Ticketing.API.Repositories.Interfaces
{
    public interface ITicketDiscussionRepository
    {
        Task<TicketDiscussionResponseDto> Create(TicketDiscussionRequestDto request);
        Task<IEnumerable<TicketDiscussionResponseDto>> GetAll(int ticketId);
        
    }
}
