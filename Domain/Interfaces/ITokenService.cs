using Domain.Models;

namespace Domain.Interfaces
{
    public interface ITokenService
    {
        int AccessTokenExpirationInSeconds { get; }
        int RefreshTokenExpirationInSeconds { get; }
        Task<string> GenerateAccessTokenAsync(User user);
        Task<string> GenerateRefreshTokenAsync();
    }
}
