using Application.ModelsDTO;
using Application.Services.Interfaces;
using ChatApi.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUsersService> _usersServiceMock;
        private readonly UsersController _sut;

        public UsersControllerTests()
        {
            _usersServiceMock = new Mock<IUsersService>();
            _sut = new UsersController(_usersServiceMock.Object);
        }

        [Fact]
        public async Task SignIn_ValidCredentials_Returns200WithTokens()
        {
            // Arrange
            var credentials = new CredentialsDTO { Login = "user@test.com", Password = "password" };
            var tokens = new TokensResponseDTO
            {
                AccessToken = new TokenDTO { Token = "access", TokenExpiresInSeconds = 900 },
                RefreshToken = new TokenDTO { Token = "refresh", TokenExpiresInSeconds = 2592000 }
            };
            _usersServiceMock.Setup(x => x.SignInAsync(credentials)).ReturnsAsync(tokens);

            // Act
            var result = await _sut.SignIn(credentials);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            okResult.Value.Should().Be(tokens);
        }

        [Fact]
        public async Task SignIn_WrongCredentials_Returns401()
        {
            // Arrange
            var credentials = new CredentialsDTO { Login = "user@test.com", Password = "wrong" };
            _usersServiceMock
                .Setup(x => x.SignInAsync(credentials))
                .ThrowsAsync(new UnauthorizedAccessException("Wrong password"));

            // Act
            var result = await _sut.SignIn(credentials);

            // Assert
            result.Result.Should().BeOfType<UnauthorizedObjectResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task Register_ValidDto_Returns201()
        {
            // Arrange
            var dto = new RegisterDTO { Username = "newuser", Email = "new@test.com", Password = "pass1234" };
            var tokens = new TokensResponseDTO
            {
                AccessToken = new TokenDTO { Token = "at", TokenExpiresInSeconds = 900 },
                RefreshToken = new TokenDTO { Token = "rt", TokenExpiresInSeconds = 2592000 }
            };
            _usersServiceMock.Setup(x => x.RegisterAsync(dto)).ReturnsAsync(tokens);

            // Act
            var result = await _sut.Register(dto);

            // Assert
            result.Result.Should().BeOfType<CreatedAtActionResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status201Created);
        }

        [Fact]
        public async Task Register_ServiceThrowsInvalidOperation_Returns400()
        {
            // Arrange
            var dto = new RegisterDTO { Username = "dup", Email = "dup@test.com", Password = "pass1234" };
            _usersServiceMock
                .Setup(x => x.RegisterAsync(dto))
                .ThrowsAsync(new InvalidOperationException("User already exists"));

            // Act
            var result = await _sut.Register(dto);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task Refresh_InvalidToken_Returns401()
        {
            // Arrange
            var tokenDto = new TokenDTO { Token = "expired_token" };
            _usersServiceMock
                .Setup(x => x.RefreshTokenAsync(tokenDto.Token))
                .ThrowsAsync(new UnauthorizedAccessException("Invalid or expired refresh token"));

            // Act
            var result = await _sut.Refresh(tokenDto);

            // Assert
            result.Result.Should().BeOfType<UnauthorizedObjectResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task Refresh_ValidToken_Returns200WithNewTokens()
        {
            // Arrange
            var tokenDto = new TokenDTO { Token = "valid_refresh" };
            var newTokens = new TokensResponseDTO
            {
                AccessToken = new TokenDTO { Token = "new_access", TokenExpiresInSeconds = 900 },
                RefreshToken = new TokenDTO { Token = "new_refresh", TokenExpiresInSeconds = 2592000 }
            };
            _usersServiceMock.Setup(x => x.RefreshTokenAsync(tokenDto.Token)).ReturnsAsync(newTokens);

            // Act
            var result = await _sut.Refresh(tokenDto);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}
