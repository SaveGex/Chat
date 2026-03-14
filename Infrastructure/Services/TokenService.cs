using Application.Options;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        public JwtOptions JwtOptions { get; set; }

        public int AccessTokenExpirationInSeconds => this.JwtOptions.AccessTokenExpirationInSeconds;
        public int RefreshTokenExpirationInSeconds => this.JwtOptions.RefreshTokenExpirationInSeconds;

        public TokenService(JwtOptions jwtOptions)
        {
            JwtOptions = jwtOptions;
        }
        public Task<string> GenerateAccessTokenAsync(User user)
        {
            List<Claim> claims = [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            ];

            var secretKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(JwtOptions.SecretKey));

            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: JwtOptions.Issuer,
                audience: JwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(JwtOptions.AccessTokenExpirationInSeconds),
                signingCredentials: signingCredentials

            );

            var tokenHandler = new JwtSecurityTokenHandler();
            string jwt = tokenHandler.WriteToken(token);

            return Task.FromResult(jwt);
        }

        public Task<string> GenerateRefreshTokenAsync()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Task.FromResult(Convert.ToBase64String(bytes));
        }
    }
}
