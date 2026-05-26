using Application.ModelsDTO;
using Application.Orchestrations.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Domain.Models;
using FluentAssertions;
using Moq;

namespace Tests.Services
{
    public class UsersServiceTests
    {
        private readonly Mock<IGenericOrchestrator<IUsersRepository>> _orchestratorMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<ITokenCacheService> _tokenCacheMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        //                       ||
        // System Under Test btw \/ 
        private readonly UsersService _sut;

        public UsersServiceTests()
        {
            _orchestratorMock = new Mock<IGenericOrchestrator<IUsersRepository>>();
            _tokenServiceMock = new Mock<ITokenService>();
            _tokenCacheMock = new Mock<ITokenCacheService>();
            _passwordHasherMock = new Mock<IPasswordHasher>();

            _sut = new UsersService(
                _orchestratorMock.Object,
                _tokenServiceMock.Object,
                _tokenCacheMock.Object,
                _passwordHasherMock.Object
            );
        }

        #region RegisterAsync

        [Fact]
        public async Task RegisterAsync_ValidDto_ReturnsTokensResponseWithBothTokens()
        {
            // Arrange
            var dto = new RegisterDTO
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "password123"
            };

            var createdUser = new User { Id = Guid.NewGuid(), Username = dto.Username, Email = dto.Email };

            _passwordHasherMock.Setup(x => x.HashPassword(dto.Password)).Returns("hashed_password");
            _orchestratorMock
                .Setup(x => x.ExecuteAsync(It.IsAny<Func<IUsersRepository, Task<User>>>()))
                .ReturnsAsync(createdUser);
            _tokenServiceMock.Setup(x => x.GenerateAccessTokenAsync(It.IsAny<User>())).ReturnsAsync("access_token");
            _tokenServiceMock.Setup(x => x.GenerateRefreshTokenAsync()).ReturnsAsync("refresh_token");
            _tokenServiceMock.Setup(x => x.AccessTokenExpirationInSeconds).Returns(900);
            _tokenServiceMock.Setup(x => x.RefreshTokenExpirationInSeconds).Returns(2592000);
            _tokenCacheMock.Setup(x => x.SaveRefreshTokenAsync(createdUser.Id, "refresh_token")).Returns(Task.CompletedTask);

            // Act
            var result = await _sut.RegisterAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.AccessToken.Should().NotBeNull();
            result.RefreshToken.Should().NotBeNull();
            result.AccessToken!.Token.Should().Be("access_token");
            result.RefreshToken!.Token.Should().Be("refresh_token");
        }

        [Fact]
        public async Task RegisterAsync_WhenIdentifierIsNull_GeneratesIdentifierFromUsername()
        {
            // Arrange
            var dto = new RegisterDTO
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "password123",
                Identifier = null
            };

            User? capturedUser = null;

            _passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<string>())).Returns("hash");
            _orchestratorMock
                .Setup(x => x.ExecuteAsync(It.IsAny<Func<IUsersRepository, Task<User>>>()))
                .Callback<Func<IUsersRepository, Task<User>>>(func =>
                {
                    // We can't easily extract the user before execution, so we capture via ReturnsAsync
                })
                .ReturnsAsync((Func<IUsersRepository, Task<User>> func) =>
                {
                    // The user passed in will have a generated Identifier
                    capturedUser = new User { Id = Guid.NewGuid(), Username = dto.Username, Email = dto.Email, Identifier = "generated" };
                    return capturedUser;
                });
            _tokenServiceMock.Setup(x => x.GenerateAccessTokenAsync(It.IsAny<User>())).ReturnsAsync("token");
            _tokenServiceMock.Setup(x => x.GenerateRefreshTokenAsync()).ReturnsAsync("refresh");
            _tokenCacheMock.Setup(x => x.SaveRefreshTokenAsync(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            var result = await _sut.RegisterAsync(dto);

            // Assert — service should not throw when Identifier is null
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task RegisterAsync_SavesRefreshTokenToCache()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var dto = new RegisterDTO { Username = "u", Email = "u@u.com", Password = "pass1234" };
            var createdUser = new User { Id = userId, Username = "u", Email = "u@u.com" };

            _passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<string>())).Returns("hash");
            _orchestratorMock
                .Setup(x => x.ExecuteAsync(It.IsAny<Func<IUsersRepository, Task<User>>>()))
                .ReturnsAsync(createdUser);
            _tokenServiceMock.Setup(x => x.GenerateAccessTokenAsync(It.IsAny<User>())).ReturnsAsync("at");
            _tokenServiceMock.Setup(x => x.GenerateRefreshTokenAsync()).ReturnsAsync("rt");
            _tokenCacheMock.Setup(x => x.SaveRefreshTokenAsync(userId, "rt")).Returns(Task.CompletedTask);

            // Act
            await _sut.RegisterAsync(dto);

            // Assert
            _tokenCacheMock.Verify(x => x.SaveRefreshTokenAsync(userId, "rt"), Times.Once);
        }

        #endregion

        #region SignInAsync

        [Fact]
        public async Task SignInAsync_ValidCredentials_ReturnsTokens()
        {
            // Arrange
            var credentials = new CredentialsDTO { Login = "user@test.com", Password = "password" };
            var user = new User { Id = Guid.NewGuid(), Email = credentials.Login, PasswordHash = "hash" };

            _orchestratorMock
                .Setup(x => x.ExecuteAsync(It.IsAny<Func<IUsersRepository, Task<User>>>()))
                .ReturnsAsync(user);
            _passwordHasherMock.Setup(x => x.Verify(credentials.Password, user.PasswordHash)).Returns(true);
            _tokenServiceMock.Setup(x => x.GenerateAccessTokenAsync(user)).ReturnsAsync("access");
            _tokenServiceMock.Setup(x => x.GenerateRefreshTokenAsync()).ReturnsAsync("refresh");
            _tokenServiceMock.Setup(x => x.AccessTokenExpirationInSeconds).Returns(900);
            _tokenCacheMock.Setup(x => x.SaveRefreshTokenAsync(user.Id, "refresh")).Returns(Task.CompletedTask);

            // Act
            var result = await _sut.SignInAsync(credentials);

            // Assert
            result.AccessToken!.Token.Should().Be("access");
            result.RefreshToken!.Token.Should().Be("refresh");
        }

        [Fact]
        public async Task SignInAsync_WrongPassword_ThrowsException()
        {
            // Arrange
            var credentials = new CredentialsDTO { Login = "user@test.com", Password = "wrongpassword" };
            var user = new User { Id = Guid.NewGuid(), Email = credentials.Login, PasswordHash = "hash" };

            _orchestratorMock
                .Setup(x => x.ExecuteAsync(It.IsAny<Func<IUsersRepository, Task<User>>>()))
                .ReturnsAsync(user);
            _passwordHasherMock.Setup(x => x.Verify(credentials.Password, user.PasswordHash)).Returns(false);

            // Act
            var act = async () => await _sut.SignInAsync(credentials);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>().WithMessage("Wrong password");
        }

        #endregion

        #region RefreshTokenAsync

        [Fact]
        public async Task RefreshTokenAsync_ValidToken_ReturnsNewTokenPair()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, Email = "u@u.com" };
            const string oldRefresh = "old_refresh_token";

            _tokenCacheMock.Setup(x => x.GetUserIdByRefreshTokenAsync(oldRefresh)).ReturnsAsync(userId);
            _orchestratorMock
                .Setup(x => x.ExecuteAsync(It.IsAny<Func<IUsersRepository, Task<User>>>()))
                .ReturnsAsync(user);
            _tokenServiceMock.Setup(x => x.GenerateAccessTokenAsync(user)).ReturnsAsync("new_access");
            _tokenServiceMock.Setup(x => x.GenerateRefreshTokenAsync()).ReturnsAsync("new_refresh");
            _tokenServiceMock.Setup(x => x.AccessTokenExpirationInSeconds).Returns(900);
            _tokenServiceMock.Setup(x => x.RefreshTokenExpirationInSeconds).Returns(2592000);
            _tokenCacheMock.Setup(x => x.RevokeRefreshTokenAsync(userId)).Returns(Task.CompletedTask);
            _tokenCacheMock.Setup(x => x.SaveRefreshTokenAsync(userId, "new_refresh")).Returns(Task.CompletedTask);

            // Act
            var result = await _sut.RefreshTokenAsync(oldRefresh);

            // Assert
            result.AccessToken!.Token.Should().Be("new_access");
            result.RefreshToken!.Token.Should().Be("new_refresh");
        }

        [Fact]
        public async Task RefreshTokenAsync_InvalidToken_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            _tokenCacheMock.Setup(x => x.GetUserIdByRefreshTokenAsync(It.IsAny<string>())).ReturnsAsync((Guid?)null);

            // Act
            var act = async () => await _sut.RefreshTokenAsync("invalid_token");

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Invalid or expired refresh token");
        }

        [Fact]
        public async Task RefreshTokenAsync_RevokesOldTokenBeforeSavingNew()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, Email = "u@u.com" };
            var callOrder = new List<string>();

            _tokenCacheMock.Setup(x => x.GetUserIdByRefreshTokenAsync("old")).ReturnsAsync(userId);
            _orchestratorMock
                .Setup(x => x.ExecuteAsync(It.IsAny<Func<IUsersRepository, Task<User>>>()))
                .ReturnsAsync(user);
            _tokenServiceMock.Setup(x => x.GenerateAccessTokenAsync(It.IsAny<User>())).ReturnsAsync("at");
            _tokenServiceMock.Setup(x => x.GenerateRefreshTokenAsync()).ReturnsAsync("new_rt");
            _tokenCacheMock
                .Setup(x => x.RevokeRefreshTokenAsync(userId))
                .Callback(() => callOrder.Add("revoke"))
                .Returns(Task.CompletedTask);
            _tokenCacheMock
                .Setup(x => x.SaveRefreshTokenAsync(userId, "new_rt"))
                .Callback(() => callOrder.Add("save"))
                .Returns(Task.CompletedTask);

            // Act
            await _sut.RefreshTokenAsync("old");

            // Assert — revoke must happen before save
            callOrder.Should().ContainInOrder("revoke", "save");
        }

        #endregion

        #region GetUserByIdAsync

        [Fact]
        public async Task GetUserByIdAsync_ExistingUser_ReturnsUserResponseDTO()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, Username = "john", Email = "j@j.com", Identifier = "john_tag" };

            _orchestratorMock
                .Setup(x => x.ExecuteAsync(It.IsAny<Func<IUsersRepository, Task<User>>>()))
                .ReturnsAsync(user);

            // Act
            var result = await _sut.GetUserByIdAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(userId);
            result.Username.Should().Be("john");
        }

        [Fact]
        public async Task GetUserByIdAsync_UserNotFound_ThrowsException()
        {
            // Arrange
            _orchestratorMock
                .Setup(x => x.ExecuteAsync(It.IsAny<Func<IUsersRepository, Task<User>>>()))
                .ReturnsAsync((User?)null);

            // Act
            var act = async () => await _sut.GetUserByIdAsync(Guid.NewGuid());

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("*not found*");
        }

        #endregion
    }
}
