using Microsoft.EntityFrameworkCore;
using Ticketing.API.Data;
using Ticketing.API.Model;
using Ticketing.API.Model.Domain;
using Ticketing.API.Model.Dto.Category;
using Ticketing.API.Repositories.Interfaces;

namespace Ticketing.API.Repositories
{
    public class CategoryRepository : BaseRepository<Category> , ICategoryRepository
    {
        private readonly TicketingDbContext dbContext;

        public CategoryRepository(TicketingDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Category>> GetList(string? status)
        {
            var data = await dbContext.Category.Where(c => c.Status == (status == "active" ? true: false)).ToListAsync();
            return data;
        }


        public async Task<Category> Create(CategoryRequestDto categoryRequest)
        {
            Category category = new Category()
            {
                Title = categoryRequest.Title,
                Status = categoryRequest.Status,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            await dbContext.Category.AddAsync(category);
            await dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<Category> Delete(int id)
        {
            var category = await dbContext.Category.FindAsync(id);
            if (category == null)
            {
                return null;
            }
            dbContext.Category.Remove(category);
            await dbContext.SaveChangesAsync();
            return category;
        }


        public async Task<Category?> Update(int id, CategoryRequestDto categoryRequest)
        {
            var category = await dbContext.Category.FindAsync(id);
            if (category == null)
            {
                return null;
            }
            category.Title = categoryRequest.Title;
            category.Status = categoryRequest.Status;
            category.UpdatedAt = DateTime.Now;
            await dbContext.SaveChangesAsync();
            return category;
        }
    }
}
