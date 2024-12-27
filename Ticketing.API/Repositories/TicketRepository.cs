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
using Ticketing.API.Services;


namespace Ticketing.API.Repositories
{
    public class TicketRepository : BaseRepository<TicketResponseDto>, ITicketRepository
    {
        private readonly IFileUploadService fileService;
        private readonly IMapper mapper;
        private static readonly string UploadDir = "Uploads/Ticket";

        public TicketRepository(TicketingDbContext dbContext , IFileUploadService fileService, IMapper mapper) : base(dbContext)
        {
            
            this.mapper = mapper;
            this.fileService = fileService;
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

        public async Task<Boolean> UploadTicketFiles(int TicketId , string Model="Ticket" , List<IFormFile> files = null)
        {
            if(files == null)
            {
                return false;
            }
            foreach(var f in files)
            {
                var uploadedFile = await fileService.UploadFile(f, Model, UploadDir);
                if(uploadedFile != null)
                {
                    var ticketFile = new TicketFile()
                    {
                        Name = uploadedFile.FileName,
                        OriginalName = uploadedFile.OriginalName,
                        MimeType = uploadedFile.Extension,
                        Path = uploadedFile.Path,
                        Size = uploadedFile.ByteSize,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        TicketId = TicketId
                    };
                    await dbContext.TicketFile.AddAsync(ticketFile);
                    await dbContext.SaveChangesAsync();
                }
            }

            return true;
        }

       

        public async Task<TicketResponseDto?> Delete(int id)
        {
            var ticket = await dbContext.Ticket
                        .Include(ticket => ticket.TicketFiles)
                        .FirstOrDefaultAsync(x => x.Id == id);

            if(ticket == null)
            {
                return null;
            }

            // Delete Uploaded Files From Uploads Directory
            if(ticket.TicketFiles != null && ticket.TicketFiles.Count > 0)
            {
                foreach(var file in ticket.TicketFiles)
                {
                    fileService.DeleteFileIfExists(UploadDir, file.Name);
                }
            }
            dbContext.Ticket.Remove(ticket);
            await dbContext.SaveChangesAsync();
            return mapper.Map<TicketResponseDto>(ticket);
        }

        public async Task<bool> DeleteTicketFile(int fileId)
        {
            var data = await dbContext.TicketFile.FindAsync(fileId);
            if (data == null) {
                return false;
            }
            fileService.DeleteFileIfExists(UploadDir, data.Name);

            dbContext.TicketFile.Remove(data);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
