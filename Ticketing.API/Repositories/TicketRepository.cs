using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Ticketing.API.Data;
using Ticketing.API.Model;
using Ticketing.API.Model.Domain;
using Ticketing.API.Model.Dto;
using Ticketing.API.Repositories.Interfaces;

namespace Ticketing.API.Repositories
{
    public class TicketRepository : BaseRepository<Ticket>, ITicketRepository
    {
        private readonly IFileRepository fileRepository;

        public TicketRepository(TicketingDbContext dbContext , IFileRepository fileRepository) : base(dbContext)
        {
            this.fileRepository = fileRepository;
        }

        public async override Task<PaginatedModel<Ticket>> GetPaginatedData(int pageNumber, int pageSize)
        {
            var rows = dbContext.Ticket
                        .Include(ticket => ticket.Category)
                        .Include(ticket => ticket.User)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .AsNoTracking();
            var data = await rows.ToListAsync();
            var totalCount = await dbContext.Ticket.CountAsync();
            var resultCount = rows.Count();
            return new PaginatedModel<Ticket>(data, totalCount, resultCount, pageNumber, pageSize);

        }

        public async override Task<Ticket?> GetById(int id)
        {
            var data =  await dbContext.Ticket
                        .Include(ticket => ticket.Category)
                        .Include(ticket => ticket.TicketFiles)
                        .ThenInclude(ticketFile => ticketFile.File)
                        .FirstOrDefaultAsync(x => x.Id == id);
            return data; 
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
