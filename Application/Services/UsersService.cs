

using Application.ModelsDTO;
using Application.Orchestrations.Interfaces;
using Application.Services.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using Mapster;
using System.Text;

namespace Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IGenericOrchestrator<IUsersRepository> _genericOrchestrator;
        private readonly ITokenService _tokenService;
        private readonly ITokenCacheService _tokenCacheService;
        private readonly IPasswordHasher _passwordHasher;

        private List<Role> BasicUserRoles { get; } = [ 
            new Role() { Name = "Guest" },
        ];

        public UsersService(
            IGenericOrchestrator<IUsersRepository> genericOrchestrator,
            ITokenService tokenService,
            ITokenCacheService tokenCacheService,
            IPasswordHasher passwordHasher)
        {
            _genericOrchestrator = genericOrchestrator;
            _tokenService = tokenService;
            _tokenCacheService = tokenCacheService;
            _passwordHasher = passwordHasher;
        }

        public async Task<TokensResponseDTO> RegisterAsync(RegisterDTO dto)
        {
            User newUser = new User()
            {
                Username = dto.Username,
                Email = dto.Email,
                Identifier = dto.Identifier ?? new Guid(Encoding.UTF8.GetBytes(dto.Username)).ToString(),
                PasswordHash = _passwordHasher.HashPassword(dto.Password),
                Roles = BasicUserRoles,
                IconUrl = null,
            };
            User createdUser = await _genericOrchestrator.ExecuteAsync(
                async usersRepository => await usersRepository.CreateUserAsync(newUser)
            );
            
            TokenDTO accessToken = new TokenDTO()
            {
                Token = await _tokenService.GenerateAccessTokenAsync(newUser),
                TokenExpiresInSeconds = _tokenService.AccessTokenExpirationInSeconds,
            };
            TokenDTO refreshToken = new TokenDTO()
            {
                Token = await _tokenService.GenerateRefreshTokenAsync(),
                TokenExpiresInSeconds = _tokenService.RefreshTokenExpirationInSeconds,
            };
            
            await _tokenCacheService.SaveRefreshTokenAsync(createdUser.Id, refreshToken.Token);
            return new TokensResponseDTO()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        public async Task<TokensResponseDTO> SignInAsync(CredentialsDTO credentials)
        {
            User user = await _genericOrchestrator.ExecuteAsync(
                async usersRepository => await usersRepository.GetUserByEmailAsync(credentials.Login)
                    ?? throw new Exception($"""User such as this login: "{credentials.Login}" - does not found""")
            );

            if(_passwordHasher.Verify(credentials.Password, user.PasswordHash) == false)
            {
                throw new Exception("Wrong password");
            }
            TokenDTO accessToken = new TokenDTO()
            {
                Token = await _tokenService.GenerateAccessTokenAsync(user),
                TokenExpiresInSeconds = _tokenService.AccessTokenExpirationInSeconds
            };
            TokenDTO refreshToken = new TokenDTO()
            {
                Token = await _tokenService.GenerateRefreshTokenAsync(),
                TokenExpiresInSeconds = _tokenService.AccessTokenExpirationInSeconds
            };

            await _tokenCacheService.SaveRefreshTokenAsync(user.Id, refreshToken.Token);
            return new TokensResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        public async Task<TokensResponseDTO> RefreshTokenAsync(string refreshToken)
        {
            Guid userId = await _tokenCacheService.GetUserIdByRefreshTokenAsync(refreshToken)
                ?? throw new UnauthorizedAccessException("Invalid or expired refresh token");

            User user = await _genericOrchestrator.ExecuteAsync(
                async usersRepository => await usersRepository.GetUserByIdAsync(userId)
                    ?? throw new UnauthorizedAccessException("User not found")
            );

            TokenDTO accessToken = new()
            {
                Token = await _tokenService.GenerateAccessTokenAsync(user),
                TokenExpiresInSeconds = _tokenService.AccessTokenExpirationInSeconds
            };
            TokenDTO newRefreshToken = new()
            {
                Token = await _tokenService.GenerateRefreshTokenAsync(),
                TokenExpiresInSeconds = _tokenService.RefreshTokenExpirationInSeconds
            };

            await _tokenCacheService.RevokeRefreshTokenAsync(userId);
            await _tokenCacheService.SaveRefreshTokenAsync(userId, newRefreshToken.Token);

            return new TokensResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task<UserResponseDTO> GetUserByIdAsync(Guid id)
        {
            var user = await _genericOrchestrator.ExecuteAsync(
                repo => repo.GetUserByIdAsync(id)
            );

            if (user is null)
                throw new Exception($"User with id: {id} - not found");

            return user.Adapt<UserResponseDTO>();
        }
    }

}
