using Application.Models;
using Application.ModelsDTO;
using Domain.Records;

namespace Application.Services.Interfaces
{
    /// <summary>
    /// Provides methods for accessing chat and group information, including retrieving available chats, identifying
    /// user participation, and obtaining group identifiers. <br/>
    /// Uses within singleton service: <see cref="Application.Orchestrations.Interfaces.IChatsAccessOrchestrator"/> - <seealso cref="Application.Orchestrations.ChatsAccessOrchestrator"/>
    /// </summary>
    /// <remarks>This interface defines asynchronous operations for querying chat data and group associations.
    /// Implementations are expected to handle pagination, filtering, and user membership checks as part of chat access
    /// functionality.</remarks>
    public interface IChatsAccessService
    {
        Task<KeysetPaginationAfterResult<ChatResponseDTO>> GetAvailableChatsAsync(string? after = null, string? propName = null, int? limit = null, bool? reverse = null);
        Task<IEnumerable<ChatResponseDTO>> ChatsContainsTheUserAsync(Guid userId);
        Task<GroupIdentifier> GetGroupIdentifierByGroupIdAsync(Guid chatId);
    }
}
