using AuthenticationAPI.DTOS;
using AuthenticationAPI.Models;

namespace AuthenticationAPI.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(ApplicationUser appUser);
    }
}
