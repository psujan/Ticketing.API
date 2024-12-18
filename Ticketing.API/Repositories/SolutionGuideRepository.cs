using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections;
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
        private readonly IFileRepository fileRepository;
        private readonly IUserManagerRepository uRep;
        private readonly IMapper mapper;
        private TicketingDbContext dbContext;

        public SolutionGuideRepository(TicketingDbContext dbContext , IFileRepository fileRepository , IUserManagerRepository uRep , IMapper mapper) 
        {
            this.fileRepository = fileRepository;
            this.uRep = uRep;
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public async  Task<PaginatedModel<SolutionGuideResponseDto>> GetPaginatedData(int pageNumber, int pageSize)
        {
            var rows = dbContext.SolutionGuide
                        .Include(s => s.Files)
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

            var fileList = await AddSolutionFiles(solutionGuide.Id, request.Files);

            //Log Added FileList Somewhere For Future Reference

            //return solutionGuide;
            return mapper.Map<SolutionGuideResponseDto>(solutionGuide);
            // Add Files

        }

        public new  async Task<SolutionGuideResponseDto?> GetById(int id)
        {
            var data = await dbContext.SolutionGuide.Include(s => s.User).FirstOrDefaultAsync(x => x.Id == id);
            var mappedData = mapper.Map<SolutionGuideResponseDto>(data);
            return mappedData;
        }

        
        public async Task<SolutionGuide?> Delete(int id)
        {
            var solutionGuide =  await dbContext.FindAsync<SolutionGuide>(id);

            if(solutionGuide == null)
            {
                return null;
            }

            dbContext.SolutionGuide.Remove(solutionGuide);
            return solutionGuide;
        }

        

        public Task<Ticket?> Update(int id, SolutionGuideRequestDto request)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Model.Domain.File>?> AddSolutionFiles(int solutionGuideId , List<IFormFile>? files ,  string? modelName = "SolutionGuide", string? uploadDir = "Uploads/Solutions/")
        {
            if(files == null || files.Count == 0)
            {
                return null;
            }

            var fileList =  await fileRepository.UploadFiles(files , modelName, uploadDir , solutionGuideId);
            return fileList;

        }

       
        Task<SolutionGuideResponseDto?> ISolutionGuideRepository.Update(int id, SolutionGuideRequestDto request)
        {
            throw new NotImplementedException();
        }

        async Task<SolutionGuideResponseDto?> ISolutionGuideRepository.Delete(int id)
        {
            var data = await dbContext.SolutionGuide.FindAsync(id);
            if(data == null)
            {
                return null;
            }

            var files = dbContext.File.Where(x => x.ModelId == id).ToList();
            if(files.Count > 0)
            {
                foreach(var f in files)
                {
                    // delete file from uploads folder
                    string? filePath = f.Path;
                    string fullFilePath = Path.Combine(Directory.GetCurrentDirectory() , filePath);

                    // Check if the file exists
                    if (File.Exists(fullFilePath))
                    {
                        // Delete the file
                        File.Delete(filePath);
                    }

                    // delete file record from file table
                
                }
            }
            dbContext.File.RemoveRange(files);
            await dbContext.SaveChangesAsync();
            // delete record from solution guide table
            dbContext.SolutionGuide.Remove(data);
            await dbContext.SaveChangesAsync();
            return mapper.Map<SolutionGuideResponseDto?>(data);
        }
    }
}
