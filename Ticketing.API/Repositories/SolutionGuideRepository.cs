using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Net.Sockets;
using Ticketing.API.Data;
using Ticketing.API.Model;
using Ticketing.API.Model.Domain;
using Ticketing.API.Model.Dto;
using Ticketing.API.Model.Dto.Requuest;
using Ticketing.API.Repositories.Interfaces;
using Ticketing.API.Repositories.Interfaces.Auth;
using Ticketing.API.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;
using File = System.IO.File;

namespace Ticketing.API.Repositories
{
    public class SolutionGuideRepository : ISolutionGuideRepository
    {
        private readonly IFileUploadService fileService;
        private readonly IUserManagerRepository uRep;
        private readonly IMapper mapper;
        private TicketingDbContext dbContext;
        private readonly static string UploadDir = "Uploads/SolutionGuide/";

        public SolutionGuideRepository(TicketingDbContext dbContext ,IFileUploadService fileService , IUserManagerRepository uRep , IMapper mapper) 
        {
            this.fileService = fileService;
            this.uRep = uRep;
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public async  Task<PaginatedModel<SolutionGuideResponseDto>> GetPaginatedData(int pageNumber, int pageSize)
        {
            var rows = dbContext.SolutionGuide
                        .Include(s => s.User)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .AsNoTracking();
            var data = await rows.ToListAsync();
            var mappedData = mapper.Map<IEnumerable<SolutionGuideResponseDto>>(data);
            var totalCount = await dbContext.SolutionGuide.CountAsync();
            var resultCount = rows.Count();
            return new PaginatedModel<SolutionGuideResponseDto>(mappedData, totalCount, resultCount, pageNumber, pageSize);
        }

        public async Task<SolutionGuideResponseDto> Create(SolutionGuideRequestDto request)
        {
            var user = await uRep.GetUserByUserName(request.UserName);
            SolutionGuide solutionGuide =  new SolutionGuide()
            {
                Title = request.Title,
                Details = request.Details,
                Status = request.Status,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                UserId = user.Id,
            };
            await dbContext.AddAsync(solutionGuide);
            await dbContext.SaveChangesAsync();

            var fileList = await UploadSolutionGuideFiles(solutionGuide.Id, "SolutionGuide" , request.Files);


            //return solutionGuide;
            return mapper.Map<SolutionGuideResponseDto>(solutionGuide);
            // Add Files

        }

        public new  async Task<SolutionGuideResponseDto?> GetById(int id)
        {
            var data = await dbContext.SolutionGuide
                .Include(s => s.User)
                .Include(sg => sg.SolutionGuideFiles)
                .FirstOrDefaultAsync(x => x.Id == id);
            var mappedData = mapper.Map<SolutionGuideResponseDto>(data);
            return mappedData;
        }

        
         
        async Task<SolutionGuideResponseDto?> ISolutionGuideRepository.Delete(int id)
        {
            var data = await dbContext.SolutionGuide.Include(sg => sg.SolutionGuideFiles)
                        .FirstOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                return null;
            }

            // Delete Uploaded Files From Uploads Directory
            if (data.SolutionGuideFiles != null && data.SolutionGuideFiles.Count > 0)
            {
                foreach (var file in data.SolutionGuideFiles)
                {
                    fileService.DeleteFileIfExists(UploadDir, file.Name);
                }
            }
            dbContext.SolutionGuide.Remove(data);
            await dbContext.SaveChangesAsync();
            return mapper.Map<SolutionGuideResponseDto?>(data);
        }

        public async Task<bool> UploadSolutionGuideFiles(int modelId, string Model, List<IFormFile> files = null)
        {

            if (files == null)
            {
                return false;
            }
            foreach (var f in files)
            {
                var uploadedFile = await fileService.UploadFile(f, Model, UploadDir);
                if (uploadedFile != null)
                {
                    var solutionGuideFile = new SolutionGuideFile()
                    {
                        Name = uploadedFile.FileName,
                        OriginalName = uploadedFile.OriginalName,
                        MimeType = uploadedFile.Extension,
                        Path = uploadedFile.Path,
                        Size = uploadedFile.ByteSize,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        SolutionGuideId = modelId
                    };
                    await dbContext.SolutionGuideFile.AddAsync(solutionGuideFile);
                    await dbContext.SaveChangesAsync();
                }
            }

            return true;
        }

        public async Task<SolutionGuideResponseDto?> Update(int id, SolutionGuideRequestDto request)
        {
            var user = await uRep.GetUserByUserName(request.UserName);
            if(user == null)
            {
                return null;
            }

            var solutionGuide = await dbContext.SolutionGuide.FindAsync(id);
            if (solutionGuide == null)
            {
                return null;
            }

            if (request.Files != null)
            {
                await UploadSolutionGuideFiles(solutionGuide.Id, "SolutionGuide", request.Files);

            }

            // Update Ticket Domain
            solutionGuide.Title = request.Title;
            solutionGuide.Details = request.Details;
            solutionGuide.Status = request.Status;
            solutionGuide.UserId = user.Id;
            solutionGuide.UpdatedAt = DateTime.Now;
            await dbContext.SaveChangesAsync();


            return mapper.Map<SolutionGuideResponseDto>(solutionGuide);
        }
    }
}
