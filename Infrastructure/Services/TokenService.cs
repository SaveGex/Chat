using Application.Options;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        public JwtOptions JwtOptions { get; set; }

        public int AccessTokenExpirationInSeconds => this.JwtOptions.AccessTokenExpirationInSeconds;

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
            throw new NotImplementedException();
        }

        public Task<bool> ValidateRefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
