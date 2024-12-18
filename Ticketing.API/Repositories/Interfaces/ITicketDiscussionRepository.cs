using Ticketing.API.Model.Domain;
using Ticketing.API.Model.Dto.Category;
using Ticketing.API.Model.Dto.Requuest;

namespace Ticketing.API.Repositories.Interfaces
{
    public interface ITicketDiscussionRepository
    {
        Task<TicketDiscussion> Create(TicketDiscussionRequestDto request);

        //Task<Category?> Update(int id, CategoryRequestDto categoryRequest);

        //Task<Category?> Delete(int id);

        Task<IEnumerable<TicketDiscussion>> GetAll(int ticketId);
    }
}
