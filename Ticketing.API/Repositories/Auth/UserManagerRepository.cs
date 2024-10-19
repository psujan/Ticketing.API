using Microsoft.AspNetCore.Identity;
using Ticketing.API.Model.Dto;
using Ticketing.API.Repositories.Interfaces.Auth;

namespace Ticketing.API.Repositories.Auth
{
    public class UserManagerRepository : IUserManagerRepository
    {
        private readonly UserManager<IdentityUser> userManager;
        private static readonly string DefaultRole = "User";
        public UserManagerRepository(UserManager<IdentityUser> userManager) 
        {
            this.userManager = userManager;
        }
        public async Task<IdentityUser?> RegisterUser(RegisterRequestDto request)
        {
            var identityUser = new IdentityUser
            {
                UserName = request.UserName,
                Email = request.UserName
            };

            var identityResult = await userManager.CreateAsync(identityUser ,request.Password);

            if (!identityResult.Succeeded)
            {
                return null;
            }

            // Add Role to User
            identityResult = await userManager.AddToRoleAsync(identityUser, DefaultRole);

            if (!identityResult.Succeeded)
            {
                return null;
            }

            return identityUser;
            
        }

        public async Task<Boolean> CheckPassword(IdentityUser user , string password)
        {
            bool passwordMatch = await userManager.CheckPasswordAsync(user, password);
            return passwordMatch;
        }

        public async Task<List<string>?> GetRoles(IdentityUser user)
        {
            var roles = await userManager.GetRolesAsync(user);

            if(roles == null)
            {
                return null;
            }

            return roles.ToList();
        }

        public async Task<IdentityUser?> GetUserByUserName(string userName)
        {
            var user =  await userManager.FindByEmailAsync(userName);
            return user;
        }

    }
}
