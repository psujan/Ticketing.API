using Microsoft.EntityFrameworkCore;
using Ticketing.API.Data;
using Ticketing.API.Model;
using Ticketing.API.Repositories.Interfaces;

namespace Ticketing.API.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly TicketingDbContext dbContext;

        public BaseRepository(TicketingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var data = await dbContext.Set<T>().AsNoTracking().ToListAsync();
            return data;
        }

        public async Task<T?> GetById(int id)
        {
            var data = await dbContext.Set<T>().FindAsync(id);
            if (data == null)
            {
                return null;
            }

            return data;
        }

        public async Task<PaginatedModel<T>> GetPaginatedData(int pageNumber, int pageSize)
        {
            var query = dbContext.Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking();

            var data = await query.ToListAsync();
            var totalCount = await dbContext.Set<T>().CountAsync();
            var resultCount =  data.Count();

            return new PaginatedModel<T>(data, totalCount , resultCount);
        }
    }
}
