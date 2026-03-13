
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        public SchoolChatContext Db { get; init; }
        public UsersRepository(SchoolChatContext db)
        {
            Db = db;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            Db.Users.Add(user);
            await Db.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
            => await Db.Users.SingleAsync(u => u.Email == email);

        public async Task<User?> GetUserByIdAsync(Guid userId)
            => await Db.Users.SingleAsync(u => u.Id == userId);
    }
}
