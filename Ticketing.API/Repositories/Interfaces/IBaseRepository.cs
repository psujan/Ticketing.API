using System.Linq.Expressions;
using Ticketing.API.Model;

namespace Ticketing.API.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T: class 
    {
        Task<IEnumerable<T>> GetAll();

        Task<T?> GetById(int id);

        Task<PaginatedModel<T>> GetPaginatedData(int pageNumber, int  pageSize);
    }
}
