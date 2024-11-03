using AutoMapper;
using System.Collections;
using Ticketing.API.Data;
using Ticketing.API.Model.Domain;
using Ticketing.API.Model.Dto;
using Ticketing.API.Repositories.Interfaces;
using Ticketing.API.Repositories.Interfaces.Auth;
using Ticketing.API.Services;

namespace Ticketing.API.Repositories
{
    public class SolutionGuideRepository : BaseRepository<SolutionGuide>, ISolutionGuideRepository
    {
        private readonly IFileRepository fileRepository;
        private readonly IUserManagerRepository uRep;
        private readonly IMapper mapper;

        public SolutionGuideRepository(TicketingDbContext dbContext , IFileRepository fileRepository , IUserManagerRepository uRep , IMapper mapper) : base(dbContext)
        {
            this.fileRepository = fileRepository;
            this.uRep = uRep;
            this.mapper = mapper;
        }

        public async Task<SolutionGuide> Create(SolutionGuideRequestDto request)
        {
            var user = await uRep.GetUserByUserName("super.admin@gmail.com");
            var mappedUser = mapper.Map<UserResponseDto>(user);
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

            var fileList = AddSolutionFiles(solutionGuide.Id, request.Files);

            //Log Added FileList Somewhere For Future Reference

            return solutionGuide;
            // Add Files

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

        Task<SolutionGuide?> ISolutionGuideRepository.Update(int id, SolutionGuideRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}
