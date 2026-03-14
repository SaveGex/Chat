using Application.Options;
using Domain.Models;
using FluentAssertions;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace Unit_Tests.Services
{
    public class TokenServiceTests
    {
        private readonly TokenService _sut;
        private readonly JwtOptions _jwtOptions;

        public TokenServiceTests()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Jwt:SecretKey"] = "super-secret-key-for-unit-tests-only-32chars",
                    ["Jwt:Issuer"] = "TestIssuer",
                    ["Jwt:Audience"] = "TestAudience",
                    ["Jwt:AccessTokenExpirationInSeconds"] = "900",
                    ["Jwt:RefreshTokenExpirationInSeconds"] = "2592000"
                })
                .Build();

            _jwtOptions = new JwtOptions(config);
            _sut = new TokenService(_jwtOptions);
        }

        [Fact]
        public async Task GenerateAccessTokenAsync_ValidUser_ReturnsNonEmptyToken()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Email = "test@test.com", Username = "tester" };

            // Act
            var token = await _sut.GenerateAccessTokenAsync(user);

            // Assert
            token.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task GenerateAccessTokenAsync_ContainsSubClaim()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, Email = "test@test.com", Username = "tester" };

            // Act
            var token = await _sut.GenerateAccessTokenAsync(user);

            // Assert
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var sub = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            sub.Should().Be(userId.ToString());
        }

        [Fact]
        public async Task GenerateRefreshTokenAsync_ReturnsDifferentTokenEachCall()
        {
            // Act
            var token1 = await _sut.GenerateRefreshTokenAsync();
            var token2 = await _sut.GenerateRefreshTokenAsync();

            // Assert
            token1.Should().NotBe(token2);
        }

        [Fact]
        public async Task GenerateRefreshTokenAsync_ReturnsBase64String()
        {
            // Act
            var token = await _sut.GenerateRefreshTokenAsync();

            // Assert — should be valid base64
            var act = () => Convert.FromBase64String(token);
            act.Should().NotThrow();
        }
    }
}
