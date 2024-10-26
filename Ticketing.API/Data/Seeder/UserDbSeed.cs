using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Ticketing.API.Data.Seeder
{
    public static class UserDbSeed
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            await SeedUser(serviceProvider);
            Console.WriteLine("User Seeding Succesful");
        }

        public static async Task SeedUser(IServiceProvider serviceProvider)
        {

            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string userName = "super.admin@gmail.com";
            string password = "Helpmate@2024";

            // Optional: Seed role
            string roleName = "SuperAdmin";
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // Check if the user already exists
            if (await userManager.FindByEmailAsync(userName) == null)
            {
                var user = new IdentityUser { UserName = userName, Email = userName, EmailConfirmed = true };
                var result = await userManager.CreateAsync(user, password);

                // Assign the user to a role (optional)
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }

                Console.WriteLine("SuperAdmin Created Successfully");
            }
        }
    }
}
