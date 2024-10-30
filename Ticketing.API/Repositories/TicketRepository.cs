using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net.Sockets;
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
            var tFiles = ticketRequestDto.Files;
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
            if(ticketRequestDto.Files != null)
            {
                var ticketFiles = await UploadTicketFiles(ticket.Id, "Ticket" , ticketRequestDto.Files);
            }
            return ticket;
        }

        public async Task<Ticket?> Update(int id, TicketRequestDto ticketRequestDto)
        {
            var ticket = await dbContext.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return null;
            }

            if(ticketRequestDto.Files != null)
            {
                await UploadTicketFiles(ticket.Id , "Ticket" , ticketRequestDto.Files);

            }

            // Update Ticket Domain
            ticket.Title = ticketRequestDto.Title;
            ticket.Details = ticketRequestDto.Details;
            ticket.Status = ticketRequestDto.Status;
            ticket.IssuerEmail = ticketRequestDto.IssuerEmail;
            ticket.IssuerPhone = ticketRequestDto.IssuerPhone;
            ticket.UserId = ticketRequestDto.UserId;
            await dbContext.SaveChangesAsync();


            return ticket;
        }

        public async Task<List<TicketFile>?> UploadTicketFiles(int TicketId , string Model="Ticket" , List<IFormFile> files = null)
        {
            if(files == null)
            {
                return null;
            }

            var fileList = await fileRepository.UploadFiles(files, Model , "Uploads/Tickets/");
            var ticketFiles = new List<TicketFile>();
            foreach (var file in fileList)
            {
                ticketFiles.Add
                (
                    new TicketFile()
                    {
                        TicketId = TicketId,
                        FileId = file.Id,
                    }
                );
            }
            await dbContext.TicketFile.AddRangeAsync(ticketFiles);
            await dbContext.SaveChangesAsync();
            return ticketFiles;
        }

        public async Task<Ticket ?> Delete(int id)
        {
            var ticket = await dbContext.Ticket
                        .Include(ticket => ticket.TicketFiles)
                        .ThenInclude(ticketFile => ticketFile.File)
                        .FirstOrDefaultAsync(x => x.Id == id);
            if (ticket == null)
            {
                return null;
            }

            // Delete Files From Storage
            if(ticket.TicketFiles !=  null && ticket.TicketFiles.Count > 0)
            {
                foreach(var f in ticket.TicketFiles)
                {
                    if(f.File != null)
                    {
                        fileRepository.DeleteFile("Uploads/Tickets/" , f.File.Name);
                    }

                }
            }
            dbContext.Ticket.Remove(ticket);
            await dbContext.SaveChangesAsync();
            return ticket;
        }
    }
}
