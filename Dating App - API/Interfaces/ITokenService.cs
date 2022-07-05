using Dating_App___API.Entities;

namespace Dating_App___API.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
