using Microsoft.EntityFrameworkCore;
using Ticketing.API.Model.Domain;

namespace Ticketing.API.Data
{
    public class TicketingDbContext : DbContext
    {

        public DbSet<Category> Category { get; set; }
        public TicketingDbContext(DbContextOptions<TicketingDbContext> dbContextOptions):base(dbContextOptions)
        {
           
        }
    }
}
