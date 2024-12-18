using Ticketing.API.Model;
using Ticketing.API.Model.Domain;
using Ticketing.API.Model.Dto;
using Ticketing.API.Model.Dto.Requuest;

namespace Ticketing.API.Repositories.Interfaces
{
    public interface ISolutionGuideRepository 
    {
        Task<SolutionGuideResponseDto> Create(SolutionGuideRequestDto request);
        Task<SolutionGuideResponseDto?> Update(int id , SolutionGuideRequestDto request);
        Task<SolutionGuideResponseDto?> Delete(int id);

        Task<SolutionGuideResponseDto?> GetById(int id);
        Task<PaginatedModel<SolutionGuideResponseDto>> GetPaginatedData(int pageNumber, int pageSize);

    }
}
