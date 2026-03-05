
namespace Domain.Interfaces
{
    public interface ITokenCacheService
    {
        Task SaveRefreshTokenAsync(Guid userId, string refreshToken);
        Task RevokeRefreshTokenAsync(Guid userId);
        Task<Guid?> GetUserIdByRefreshTokenAsync(string refreshToken);
    }
}
