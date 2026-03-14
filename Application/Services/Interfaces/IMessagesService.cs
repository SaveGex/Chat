using Application.ModelsDTO;
using Domain.Records;

namespace Application.Services.Interfaces
{
    public interface IMessagesService
    {
        Task<MessageResponseDTO> CreateMessageAsync(MessageCreateDTO dto);

        /// <summary>
        /// Getting <see cref="KeysetPaginationAfterResult{T}"/> (cursor pagination) of the chat (by <paramref name="chatId"/>)
        /// </summary>
        /// <param name="after">After row</param>
        /// <param name="propName">Sort by prop name(default by time)</param>
        /// <param name="limit">max count of elements in keyset(default 20)</param>
        /// <param name="reverse">reverse the collection? (default <see cref="false"/>/></param>
        /// <returns></returns>
        Task<KeysetPaginationAfterResult<MessageResponseDTO>> GetMessagesKeysetPaginationAsync(string? after = null, string? propName = null, int? limit = null, bool? reverse = null);

        /// <summary>
        /// Updating existing message by id
        /// </summary>
        /// <param name="messageId">Message id to update</param>
        /// <param name="dto">new values of existing message</param>
        /// <returns></returns>
        Task<MessageResponseDTO> UpdateMessageAsync(Guid messageId, MessageUpdateDTO dto);

        Task<MessageResponseDTO> DeleteMessageAsync(Guid messageId);

    }
}
