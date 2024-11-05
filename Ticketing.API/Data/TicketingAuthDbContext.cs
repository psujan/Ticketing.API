using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Ticketing.API.Model.Domain;

namespace Ticketing.API.Data
{
    public class TicketingAuthDbContext : IdentityDbContext<User>
    {
        public DbSet<User> User { get; set; }
        
        public TicketingAuthDbContext(DbContextOptions<TicketingAuthDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            
            base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("AspNetUsers");


            var roles = new List<IdentityRole> {
                new IdentityRole
                {
                    Id = "1",
                    ConcurrencyStamp = "1",
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin".ToUpper()
                },
                new IdentityRole
                {
                    Id = "2",
                    ConcurrencyStamp = "2",
                    Name = "Moderator",
                    NormalizedName = "Moderator".ToUpper()
                },
                new IdentityRole
                {
                    Id = "3",
                    ConcurrencyStamp = "3",
                    Name = "User",
                    NormalizedName = "User".ToUpper()
                },
                new IdentityRole
                {
                    Id = "4",
                    ConcurrencyStamp = "4",
                    Name = "Visitor",
                    NormalizedName = "Visitor".ToUpper()
                },
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
