using Application.ModelsDTO;

namespace Application.Services.Interfaces
{
    public interface IUsersService
    {
        Task<UserResponseDTO> GetUserByIdAsync(Guid id);
        Task<TokensResponseDTO> RegisterAsync(RegisterDTO dto);
        Task<TokensResponseDTO> SignInAsync(CredentialsDTO credentials);
        Task<TokensResponseDTO> RefreshTokenAsync(string refreshToken);
    }
}