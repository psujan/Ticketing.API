using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq.Expressions;
using System.Net.Sockets;
using Ticketing.API.Data;
using Ticketing.API.Model;
using Ticketing.API.Model.Domain;
using Ticketing.API.Model.Dto;
using Ticketing.API.Model.Dto.Requuest;
using Ticketing.API.Repositories.Interfaces;
using TicketResponseDto = Ticketing.API.Model.Dto.TicketResponseDto;
using Ticket = Ticketing.API.Model.Domain.Ticket;


namespace Ticketing.API.Repositories
{
    public class TicketRepository : BaseRepository<TicketResponseDto>, ITicketRepository
    {
        private readonly IFileRepository fileRepository;
        private readonly IMapper mapper;

        public TicketRepository(TicketingDbContext dbContext , IFileRepository fileRepository , IMapper mapper) : base(dbContext)
        {
            this.fileRepository = fileRepository;
            this.mapper = mapper;
        }

        public async override Task<PaginatedModel<TicketResponseDto>> GetPaginatedData(int pageNumber, int pageSize)
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
            var mappedData = mapper.Map<IEnumerable<TicketResponseDto>>(data);
            return new PaginatedModel<TicketResponseDto>(mappedData, totalCount, resultCount, pageNumber, pageSize);

        }

        public async new Task<TicketResponseDto> GetById(int id)
        {
            var data =  await dbContext.Ticket
                        .Include(ticket => ticket.Category)
                        .Include(ticket => ticket.TicketFiles)
                        .ThenInclude(ticketFile => ticketFile.File)
                        .FirstOrDefaultAsync(x => x.Id == id);
            return mapper.Map<TicketResponseDto>(data); ; 
        }

        public async Task<TicketResponseDto> Create(TicketRequestDto ticketRequestDto)
        {
            var tFiles = ticketRequestDto.Files;
            Ticket ticket = new Ticket()
            {
                Title = ticketRequestDto.Title,
                Details = ticketRequestDto.Details,
                Status  = ticketRequestDto.Status,
                IssuerEmail = ticketRequestDto.IssuerEmail,
                IssuerPhone = ticketRequestDto.IssuerPhone,
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
                await UploadTicketFiles(ticket.Id, "Ticket" , ticketRequestDto.Files);
            }
            return mapper.Map<TicketResponseDto>(ticket);
        }

        public async Task<TicketResponseDto?> Update(int id, TicketRequestDto ticketRequestDto)
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
            ticket.UpdatedAt = DateTime.Now;
            await dbContext.SaveChangesAsync();


            return mapper.Map<TicketResponseDto>(ticket);
        }

        public async Task<TicketResponseDto?> UpdateStatus(int id , string status)
        {
            var ticket = await dbContext.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return null;
            }
            ticket.Status = status;
            await dbContext.SaveChangesAsync();
            return mapper.Map<TicketResponseDto>(ticket);

        }

        public async Task<IEnumerable<Model.Domain.File>?> UploadTicketFiles(int TicketId , string Model="Ticket" , List<IFormFile> files = null)
        {
            if(files == null)
            {
                return null;
            }

            var  fileList = await fileRepository.UploadFiles(files, "Ticket" , "Uploads/Tickets/" , TicketId);
            await SaveTicketFiles(TicketId , fileList);
            return fileList;
        }

        public async Task<IEnumerable<TicketFile>> SaveTicketFiles(int TicketId , IEnumerable<Model.Domain.File> fileList)
        {
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

        public async Task<TicketResponseDto ?> Delete(int id)
        {
            var ticket = await dbContext.Ticket
                        //.Include(ticket => ticket.Files)
                        .FirstOrDefaultAsync(x => x.Id == id);
            if (ticket == null)
            {
                return null;
            }

            // Delete Uploaded Files From Storage and Database
            /*if(ticket.Files !=  null && ticket.Files.Count > 0)
            {
                foreach(var f in ticket.Files)
                {
                    if(f != null)
                    {
                        await fileRepository.DeleteFile("Uploads/Tickets/" , f.Name , "Ticket", (int)f.ModelId);
                    }

                }
            }*/
            dbContext.Ticket.Remove(ticket);
            await dbContext.SaveChangesAsync();
            return mapper.Map<TicketResponseDto>(ticket);
        }
    }
}
