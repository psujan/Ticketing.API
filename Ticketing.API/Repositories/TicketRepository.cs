using Ticketing.API.Data;
using Ticketing.API.Model.Domain;
using Ticketing.API.Model.Dto;
using Ticketing.API.Repositories.Interfaces;

namespace Ticketing.API.Repositories
{
    public class TicketRepository : BaseRepository<Ticket>, ITicketRepository
    {
        private readonly TicketingDbContext dbContext;

        public TicketRepository(TicketingDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Ticket> Create(TicketRequestDto ticketRequestDto)
        {
            Ticket ticket = new Ticket()
            {
                Title = ticketRequestDto.Title,
                Details = ticketRequestDto.Details,
                Status  = ticketRequestDto.Status,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
               // UserId = ticketRequestDto.UserId,
                //CategoryId = ticketRequestDto.CategoryId,
            };

            await dbContext.Ticket.AddAsync(ticket);
            await dbContext.SaveChangesAsync();
            return ticket;
        }
    }
}
