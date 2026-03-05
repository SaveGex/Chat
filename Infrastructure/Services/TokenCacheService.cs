
using Application.Options;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Services
{
    public class TokenCacheService : ITokenCacheService
    {
        private IMemoryCache MemoryCache { get; }
        private JwtOptions JwtOptions { get; }

        public TokenCacheService(IMemoryCache memoryCache, JwtOptions jwtOptions)
        {
            MemoryCache = memoryCache;
            JwtOptions = jwtOptions;
        }

        public async Task<Guid?> GetUserIdByRefreshTokenAsync(string refreshToken)
        {
            _ = MemoryCache.TryGetValue(refreshToken, out Guid userId);
            return userId;
        }

        public Task RevokeRefreshTokenAsync(Guid userId)
        {
            MemoryCache.Remove(userId);
            return Task.CompletedTask;
        }

        public Task SaveRefreshTokenAsync(Guid userId, string refreshToken)
        {
            MemoryCache.Set(refreshToken, userId, TimeSpan.FromSeconds(JwtOptions.RefreshTokenExpirationInSeconds));
            return Task.CompletedTask;
        }
    }
}
