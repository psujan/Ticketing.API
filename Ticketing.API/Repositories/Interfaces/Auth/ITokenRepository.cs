using Ticketing.API.Model.Domain;

namespace Ticketing.API.Repositories.Interfaces.Auth
{
    public interface ITokenRepository
    {
        string CreateJWTToken(User user, List<string> roles);
    }
}
