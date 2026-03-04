using Domain.Models;
using Domain.Records;

namespace Domain.Interfaces
{
    public interface IMessagesRepository
    {

        Task<Message> CreateMessageAsync(Message message);
        Task<Message?> GetMessageByIdAsync(Guid messageId);
        Task<KeysetPaginationAfterResult<Message>> GetMessagesKeysetPaginationAsync(string? after, string propName, int limit, bool IsDescending);
        Task<Message> UpdateMessageAsync(Guid messageId, Message message);
        Task<Message> DeleteMessageAsync(Guid messageId);

    }
}
