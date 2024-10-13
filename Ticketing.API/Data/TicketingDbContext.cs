using Microsoft.EntityFrameworkCore;

namespace Ticketing.API.Data
{
    public class TicketingDbContext : DbContext
    {
        public TicketingDbContext(DbContextOptions dbContextOptions):base(dbContextOptions)
        {
            
        }
    }
}
