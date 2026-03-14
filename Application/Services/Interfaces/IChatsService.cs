using Application.ModelsDTO;

namespace Application.Services.Interfaces;

/// <summary>
/// Defines operations for CRUD chat entities asynchronously.
/// </summary>
/// <remarks>Implementations of this interface should ensure thread safety for concurrent operations. All methods
/// return tasks that complete with the result of the corresponding chat operation. The interface abstracts chat
/// management functionality and is intended to be used in application layers that require chat-related
/// actions.</remarks>
public interface IChatsService
{
    Task<ChatResponseDTO> CreateChatAsync(ChatCreateDTO dto);
    Task<ChatResponseDTO> GetChatAsync(Guid chatId);
    Task<ChatResponseDTO> UpdateChatAsync(Guid chatId, ChatUpdateDTO dto);
    Task<ChatResponseDTO> DeleteChatAsync(Guid chatId);
}
