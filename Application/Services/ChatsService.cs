using Application.ModelsDTO;
using Application.Services.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using Mapster;

namespace Application.Services
{
    public class ChatsService : IChatsService
    {
        private IChatsCRUDRepository CRUDRepository { get; init; }

        public ChatsService(IChatsCRUDRepository chatsRepository)
        {
            CRUDRepository = chatsRepository;
        }

        public async Task<ChatResponseDTO> CreateChatAsync(ChatCreateDTO dto)
        {
            var chat = dto.Adapt<Chat>();
            var result = await CRUDRepository.CreateChatAsync(chat);
            return result.Adapt<ChatResponseDTO>();
        }


        public async Task<ChatResponseDTO> GetChatAsync(Guid chatId)
        {
            var chat = await CRUDRepository.GetChatByIdAsync(chatId);
            return chat.Adapt<ChatResponseDTO>();
        }
        public async Task<ChatResponseDTO> UpdateChatAsync(Guid chatId, ChatUpdateDTO dto)
        {
            var chat = dto.Adapt<Chat>();
            var result = await CRUDRepository.UpdateChatAsync(chatId, chat);
            return result.Adapt<ChatResponseDTO>();
        }

        public async Task<ChatResponseDTO> DeleteChatAsync(Guid chatId)
        {
            var chat = await CRUDRepository.DeleteChatAsync(chatId);
            return chat.Adapt<ChatResponseDTO>();
        }
    }
}
