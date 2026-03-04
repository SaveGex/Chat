
namespace Domain.Interfaces
{
    public interface ITokenCacheService
    {
        Task SaveRefreshTokenAsync(Guid userId, string refreshToken);
        Task<string?> GetRefreshTokenAsync(string userId);
        Task RevokeRefreshTokenAsync(string userId);
    }
}
