using Microsoft.AspNetCore.Identity;
using Ticketing.API.Model.Dto;

namespace Ticketing.API.Repositories.Interfaces.Auth
{
    public interface IUserManagerRepository
    {
        Task<IdentityUser?> RegisterUser(RegisterRequest request);
        Task<IdentityUser?> GetUserByUserName(string userName);
        Task<Boolean> CheckPassword(IdentityUser user, string password);
        Task<List<string>?> GetRoles(IdentityUser user);

    }
}
