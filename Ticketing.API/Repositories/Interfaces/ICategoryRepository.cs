using Ticketing.API.Model.Domain;
using Ticketing.API.Model.Dto.Category;

namespace Ticketing.API.Repositories.Interfaces
{
    public interface ICategoryRepository: IBaseRepository<Category>
    {
        Task<Category> Create(CategoryRequestDto categoryRequest);

        Task<Category?> Update(int id , CategoryRequestDto categoryRequest);

        Task<Category?> Delete(int id);

        Task<IEnumerable<Category>> GetList(string? status);
    }
}
