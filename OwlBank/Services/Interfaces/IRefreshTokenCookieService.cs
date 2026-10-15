using OwlBank.Models;

namespace OwlBank.Services.Interfaces
{
    public interface IRefreshTokenCookieService
    {
        void SetRefreshToken(User user);
        string GenerateToken(User user);
    }
}