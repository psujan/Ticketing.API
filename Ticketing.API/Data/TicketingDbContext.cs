using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ticketing.API.Model.Domain;

namespace Ticketing.API.Data
{
    public class TicketingDbContext : IdentityDbContext<User>
    {

        public DbSet<User> User { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<Model.Domain.TicketFile> TicketFile { get; set; }

        public DbSet<Model.Domain.File> File { get; set; }
        public DbSet<Model.Domain.SolutionGuide> SolutionGuide { get; set; }


        public TicketingDbContext(DbContextOptions<TicketingDbContext> dbContextOptions):base(dbContextOptions)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ticket>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .IsRequired(false);

            modelBuilder.Entity<TicketFile>()
                .HasOne(e => e.Ticket)
                .WithMany(t => t.TicketFiles)
                .HasForeignKey(t => t.TicketId)
                .IsRequired(true);

            modelBuilder.Entity<TicketFile>()
                .HasOne(e => e.File)
                .WithOne(e => e.TicketFile)
                .HasForeignKey<TicketFile>(e => e.FileId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SolutionGuide>()
                .HasMany(e => e.Files)
                .WithOne(t => t.SolutionGuide)
                .HasForeignKey(e => e.ModelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TicketDiscussion>()
                .HasOne(e => e.Ticket)
                .WithMany(t => t.TicketDiscussions)
                .HasForeignKey(e => e.TicketId);

            modelBuilder.Entity<TicketDiscussion>()
                .HasOne(e => e.User)
                .WithMany(u => u.TicketDiscussion)
                .HasForeignKey(e => e.UserId);
                

            /*modelBuilder.Entity<SolutionGuide>()
                .HasOne(sg => sg.User)
                .WithMany() // or .WithMany(u => u.SolutionGuides) if you have a collection property
                .HasForeignKey(sg => sg.UserId);*/
                

            // Seed Role Data
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

            modelBuilder.Entity<IdentityRole>().HasData(roles);

        }
    }
}
