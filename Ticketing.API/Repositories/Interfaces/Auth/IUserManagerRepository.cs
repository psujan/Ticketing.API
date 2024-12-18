using Ticketing.API.Model.Domain;
using Ticketing.API.Model.Dto.Requuest;

namespace Ticketing.API.Repositories.Interfaces.Auth
{
    public interface IUserManagerRepository
    {
        Task<User?> RegisterUser(RegisterRequestDto request);
        Task<User?> GetUserByUserName(string userName);
        Task<Boolean> CheckPassword(User user, string password);
        Task<List<string>?> GetRoles(User user);

    }
}
