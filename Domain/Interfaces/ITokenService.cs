using Domain.Models;

namespace Domain.Interfaces
{
    public interface ITokenService
    {
        int AccessTokenExpirationInSeconds { get; }
        Task<string> GenerateAccessTokenAsync(User user);
        Task<string> GenerateRefreshTokenAsync();
        Task<bool> ValidateRefreshTokenAsync(string refreshToken);
    }
}
