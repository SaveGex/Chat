
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IChatsCRUDRepository
    {
        Task<Chat> CreateChatAsync(Chat dto);
        Task<Chat> GetChatByIdAsync(Guid chatId);
        Task<Chat> UpdateChatAsync(Guid chatId, Chat dto);
        Task<Chat> DeleteChatAsync(Guid chatId);
    }
}
