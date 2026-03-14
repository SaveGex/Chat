using Domain.Models;

namespace Domain.Interfaces
{
    public interface IUsersRepository
    {
        Task<User> CreateUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(Guid userId);
    }
}
