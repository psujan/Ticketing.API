using Ticketing.API.Model.Domain;
using Ticketing.API.Model.Dto;

namespace Ticketing.API.Repositories.Interfaces
{
    public interface ISolutionGuideRepository : IBaseRepository<SolutionGuide>
    {
        Task<SolutionGuide> Create(SolutionGuideRequestDto request);
        Task<SolutionGuide?> Update(int id , SolutionGuideRequestDto request);
        Task<SolutionGuide?> Delete(int id);
    }
}
