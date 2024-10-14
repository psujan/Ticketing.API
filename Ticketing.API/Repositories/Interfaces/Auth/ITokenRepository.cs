using Microsoft.AspNetCore.Identity;

namespace Ticketing.API.Repositories.Interfaces.Auth
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
