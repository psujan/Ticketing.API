using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ticketing.API.Model.Domain;

namespace Ticketing.API.Data
{
    public class TicketingDbContext : DbContext
    {

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

            modelBuilder.Entity<SolutionGuide>()
                .HasOne(sg => sg.User)
                .WithMany() // or .WithMany(u => u.SolutionGuides) if you have a collection property
                .HasForeignKey(sg => sg.UserId)
                .HasPrincipalKey(u => u.Id);

        }
    }
}
