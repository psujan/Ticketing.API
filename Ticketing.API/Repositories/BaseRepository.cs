using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Ticketing.API.Data;
using Ticketing.API.Model;
using Ticketing.API.Repositories.Interfaces;

namespace Ticketing.API.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        public readonly TicketingDbContext dbContext;

        public BaseRepository(TicketingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async virtual Task<IEnumerable<T>> GetAll()
        {
            var data = await dbContext.Set<T>().AsNoTracking().ToListAsync();
            return data;
        }

       

        public async virtual Task<T?> GetById(int id)
        {  
            var data = await dbContext.Set<T>().FindAsync(id);
            return data;
        }

        /*public async virtual Task<PaginatedModel<T>> GetPaginatedData(int pageNumber, int pageSize , List<Expression<Func<T, object>>>? includes = null)
        {
            IQueryable<T> query = dbContext.Set<T>();

            if(includes != null)
            {
                foreach(var include in includes)
                {
                    query = query.Include(include);
                }
            }

            var rows = query.Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                               .AsNoTracking();

            var data = await rows.ToListAsync();
            var totalCount = await dbContext.Set<T>().CountAsync();
            var resultCount =  data.Count();

            return new PaginatedModel<T>(data, totalCount , resultCount  , pageNumber , pageSize);
        }*/
        public async virtual Task<PaginatedModel<T>> GetPaginatedData(int pageNumber = 1, int pageSize = 1  )
        {
            IQueryable<T> query = dbContext.Set<T>();
           
            var rows = query.Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                               .AsNoTracking();

            var data = await rows.ToListAsync();
            var totalCount = await dbContext.Set<T>().CountAsync();
            var resultCount =  data.Count();

            return new PaginatedModel<T>(data, totalCount , resultCount  , pageNumber , pageSize);
        }
    }
}
