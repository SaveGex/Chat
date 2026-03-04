using Application.ModelsDTO;

namespace Application.Services.Interfaces
{
    public interface IUsersService
    {
        Task<TokensResponseDTO> RegisterAsync(RegisterDTO dto);
        Task<UserResponseDTO> SignInAsync(CredentialsDTO credentials);
        Task<UserResponseDTO> RefreshTokenAsync(string refreshToken);
    }
}