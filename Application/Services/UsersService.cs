

using Application.ModelsDTO;
using Application.Services.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using System.Text;

namespace Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ITokenService _tokenService;
        private readonly ITokenCacheService _tokenCacheService;
        private readonly IPasswordHasher _passwordHasher;

        private List<Role> BasicUserRoles { get; } = [ 
            new Role() { Name = "Guest" },
        ];

        public UsersService(
            IUsersRepository usersRepository,
            ITokenService tokenService,
            ITokenCacheService tokenCacheService,
            IPasswordHasher passwordHasher)
        {
            _usersRepository = usersRepository;
            _tokenService = tokenService;
            _tokenCacheService = tokenCacheService;
            _passwordHasher = passwordHasher;
        }

        public async Task<TokensResponseDTO> RegisterAsync(RegisterDTO dto)
        {
            // 1. створити юзера → IUsersRepository
            User newUser = new User()
            {
                Username = dto.Username,
                Email = dto.Email,
                Identifier = dto.Identifier ?? new Guid(Encoding.UTF8.GetBytes(dto.Username)).ToString(),
                PasswordHash = _passwordHasher.HashPassword(dto.Password),
                Roles = BasicUserRoles,
                IconUrl = null,
            };
            User createdUser = await _usersRepository.CreateUserAsync(newUser);
            // 2. згенерувати токени → ITokenService
            string accessToken = await _tokenService.GenerateAccessTokenAsync(newUser);
            string refreshToken = await _tokenService.GenerateRefreshTokenAsync();
            // 3. закешувати refresh → ITokenCacheService
            await _tokenCacheService.SaveRefreshTokenAsync(createdUser.Id, refreshToken);
            return new TokensResponseDTO()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = _tokenService.AccessTokenExpirationInSeconds,
            };
        }

        public async Task<UserResponseDTO> SignInAsync(CredentialsDTO credentials)
        {
            // 1. знайти юзера → IUsersRepository
            // 2. перевірити пароль
            // 3. згенерувати токени → ITokenService
            // 4. закешувати refresh → ITokenCacheService
            throw new NotImplementedException();
        }

        public async Task<UserResponseDTO> RefreshTokenAsync(string refreshToken)
        {
            // 1. валідувати refresh → ITokenService
            // 2. перевірити кеш → ITokenCacheService
            // 3. згенерувати нові токени → ITokenService
            // 4. оновити кеш → ITokenCacheService
            throw new NotImplementedException();
        }

    }

}
