using Microsoft.EntityFrameworkCore;

namespace Ticketing.API.Data.Seeder
{
    public static class DbSeed
    {
        public static void Initialize(TicketingDbContext dbContext)
        {
            

            if (!dbContext.Category.Any()) 
            {
                SeedCategory(dbContext);
                Console.WriteLine("Category Seeding Successful");
            }

            if (!dbContext.Ticket.Any())
            {
                SeedTicket(dbContext);
            }

            

            
        }

        public static void SeedCategory(TicketingDbContext dbContext) {
            dbContext.Category.AddRange(
                new Model.Domain.Category
                {
                    //Id = 1,
                    Title = "Delivery and Tracking",
                    Status = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt   = DateTime.Now,
                },
                 new Model.Domain.Category
                 {
                    // Id = 2,
                     Title = "Payment Processing",
                     Status = true,
                     CreatedAt = DateTime.Now,
                     UpdatedAt = DateTime.Now,
                 },
                 new Model.Domain.Category
                 {
                     //Id = 3,
                     Title = "Application and ServerIssue",
                     Status = true,
                     CreatedAt = DateTime.Now,
                     UpdatedAt = DateTime.Now,
                 }
            );

            dbContext.SaveChanges();
        }

        public static void SeedTicket(TicketingDbContext dbContext)
        {
            dbContext.Ticket.AddRange
            (
                new Model.Domain.Ticket
                {
                    //Id = 1,
                    Title = "Sample Seed Ticket. No Need To Resolve It",
                    Details = "Sample Seed Ticket. No Need To Resolve It",
                    IssuerEmail = "",
                    IssuerPhone = "",
                    CategoryId = 1,
                    Status = "Inactive",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
                new Model.Domain.Ticket
                {
                    //Id = 2,
                    Title = "Sample Seed Ticket 2. No Need To Resolve It",
                    Details = "Sample Seed Ticket 2. No Need To Resolve It",
                    IssuerEmail = "",
                    IssuerPhone = "",
                    CategoryId = 2,
                    Status = "Inactive",
                    CreatedAt   = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                }
            );

            dbContext.SaveChanges();

            //Along with ticket seed TicketFile
            dbContext.File.AddRange
            (
                new Model.Domain.File
                {
                   // Id = 1,
                    Name = "Test File",
                    Model = "Ticket",
                    CreatedAt   = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Path = "https://file-examples.com/storage/fee7a7e285671bd4a9d4d9d/2017/10/file_example_JPG_100kB.jpg"
                },
                new Model.Domain.File
                {
                    //Id = 2,
                    Name = "Test File 2",
                    Model = "Ticket",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Path = "https://file-examples.com/storage/fee7a7e285671bd4a9d4d9d/2017/10/file_example_JPG_100kB.jpg"
                }
            );

            dbContext.SaveChanges();

            dbContext.TicketFile.AddRange
            (
                new Model.Domain.TicketFile
                {
                   // Id = 1,
                    TicketId = 1,
                    FileId = 1,
                },
                new Model.Domain.TicketFile
                {
                    // Id = 2,
                     TicketId = 2,
                     FileId = 2,
                }
            );

            dbContext.SaveChanges();
        }
    }
}
