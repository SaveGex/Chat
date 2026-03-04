using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly PasswordHasher<object> _hasher = new();

        public string HashPassword(string password) =>
            _hasher.HashPassword(null!, password);

        public bool Verify(string password, string hash) =>
            _hasher.VerifyHashedPassword(null!, hash, password)
            != PasswordVerificationResult.Failed;
    }
}
