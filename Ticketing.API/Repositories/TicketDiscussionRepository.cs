using Ticketing.API.Data;
using Ticketing.API.Model.Domain;
using Ticketing.API.Model.Dto.Requuest;
using Ticketing.API.Repositories.Interfaces;

namespace Ticketing.API.Repositories
{
    public class TicketDiscussionRepository : ITicketDiscussionRepository
    {
        private readonly TicketingDbContext dbContext;

        private readonly ITicketRepository ticketRepository;

        public TicketDiscussionRepository(TicketingDbContext dbContext , ITicketRepository ticketRepository)
        {
            this.dbContext = dbContext;
            this.ticketRepository = ticketRepository;
        }


        public async Task<TicketDiscussion> Create(TicketDiscussionRequestDto request)
        {
            var ticket = await ticketRepository.GetById(request.TicketId);

            if (ticket == null) 
            {
                throw new Exception("Ticket doesnot exist");
            }

            TicketDiscussion discussion = new TicketDiscussion()
            {
                Comment = request.Comment,
                DeletedAt = request.DeletedAt,
                TicketId = request.TicketId,
                UserId = request.UserId
            };

            await dbContext.AddAsync(discussion);
            await dbContext.SaveChangesAsync();
            return discussion;
        }

        public Task<IEnumerable<TicketDiscussion>> GetAll(int ticketId)
        {
            throw new NotImplementedException();
        }
    }
}
