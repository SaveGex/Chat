
using Domain.Interfaces;

namespace Infrastructure.Services
{
    public class TokenCacheService : ITokenCacheService
    {
        public Task<string?> GetRefreshTokenAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task RevokeRefreshTokenAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task SaveRefreshTokenAsync(Guid userId, string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
