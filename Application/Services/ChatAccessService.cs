using Application.Models;
using Application.ModelsDTO;
using Application.Services.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using Domain.Records;
using Mapster;
namespace Application.Services;

public class ChatAccessService : IChatsAccessService
{
    private IChatsQueryRepository QueryRepository { get; init; }
    private IChatsCRUDRepository CRUDRepository { get; init; }

    public ChatAccessService(IChatsQueryRepository queryRepository, IChatsCRUDRepository crudRepository)
    {
        QueryRepository = queryRepository;
        CRUDRepository = crudRepository;
    }

    public async Task<KeysetPaginationAfterResult<ChatResponseDTO>> GetAvailableChatsAsync(string? after = null, string? propName = null, int? limit = null, bool? reverse = null)
    {
        string sortByProp = propName ?? nameof(Chat.Users);
        limit = limit > 1000 ? 1000 : 100;
        reverse ??= false;
        propName ??= nameof(Chat.Id);

        var ksr = await QueryRepository.GetChatKeysetPaginationAsync(after, propName, (int)limit, (bool)reverse);
        return ksr.Adapt<KeysetPaginationAfterResult<ChatResponseDTO>>();
    }

    public async Task<IEnumerable<ChatResponseDTO>> ChatsContainsTheUserAsync(Guid userId)
    {
        var chats = await QueryRepository.ChatsContainsTheUser(userId);
        return chats.Adapt<List<ChatResponseDTO>>();
    }

    public async Task<GroupIdentifier> GetGroupIdentifierByGroupIdAsync(Guid chatId)
    {
        var chat = await CRUDRepository.GetChatByIdAsync(chatId);
        return new GroupIdentifier(chat.Title, chat.Id);
    }

}
