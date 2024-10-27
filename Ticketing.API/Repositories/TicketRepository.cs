using Microsoft.EntityFrameworkCore;
using Ticketing.API.Data;
using Ticketing.API.Model.Domain;
using Ticketing.API.Model.Dto;
using Ticketing.API.Repositories.Interfaces;

namespace Ticketing.API.Repositories
{
    public class TicketRepository : BaseRepository<Ticket>, ITicketRepository
    {
        private readonly TicketingDbContext dbContext;
        private readonly IFileRepository fileRepository;

        public TicketRepository(TicketingDbContext dbContext , IFileRepository fileRepository) : base(dbContext)
        {
            this.dbContext = dbContext;
            this.fileRepository = fileRepository;
        }

        public async override Task<Ticket?> GetById(int id)
        {
            var ticket = await dbContext.Ticket
                .Include(ticket => ticket.Category)
                .Include(ticket => ticket.TicketFiles)
                .ThenInclude(ticketFile => ticketFile.File)
                .SingleOrDefaultAsync(d => d.Id == id);
            return ticket;
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
                UserId = ticketRequestDto.UserId,
                CategoryId = ticketRequestDto.CategoryId,
            };

            await dbContext.Ticket.AddAsync(ticket);
            await dbContext.SaveChangesAsync();

            //await UploadFiles();
            var fileList = await fileRepository.UploadFiles(ticketRequestDto.Files, "Ticket");
            var ticketFiles = new List<TicketFile>();
            foreach (var file in fileList) 
            {
                ticketFiles.Add
                (
                    new TicketFile()
                    {
                        TicketId = ticket.Id,
                        FileId = file.Id,
                    }
                );   
            }
            await dbContext.TicketFile.AddRangeAsync(ticketFiles);
            return ticket;
        }
    }
}
