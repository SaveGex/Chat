
using Domain.Models;
using Domain.Records;

namespace Domain.Interfaces
{
    public interface IChatsQueryRepository
    {
        Task<KeysetPaginationAfterResult<Chat>> GetChatKeysetPaginationAsync(string? after, string propName, int limit, bool IsDescending);

        Task<IEnumerable<Chat>> ChatsContainsTheUser(Guid userId);

    }
}
