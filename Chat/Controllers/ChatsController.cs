using Application.ModelsDTO;
using Application.Services.Interfaces;
using Domain.Records;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private IChatsService ChatsService { get; init; }
        private IChatsAccessService ChatAccessService { get; init; }

        public ChatsController(IChatsService chatsService, IChatsAccessService chatAccessService)
        {
            ChatsService = chatsService;
            ChatAccessService = chatAccessService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IResult> CreateChat(
            [FromBody] ChatCreateDTO dto)
        {
            try
            {
                await ChatsService.CreateChatAsync(dto);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
            return Results.Created();
        }

        [HttpGet]
        [ProducesResponseType(typeof(KeysetPaginationAfterResult<ChatResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<KeysetPaginationAfterResult<ChatResponseDTO>>> GetAvailableChatsAsync()
        {
            KeysetPaginationAfterResult<ChatResponseDTO> result;
            try
            {
                result = await ChatAccessService.GetAvailableChatsAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(result);
        }

        [HttpGet("chatId:int")]
        [ProducesResponseType(typeof(ChatResponseDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<ChatResponseDTO>> GetChat(Guid chatId)
        {
            ChatResponseDTO result;
            try
            {
                result = await ChatsService.GetChatAsync(chatId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(result);
        }

        [HttpDelete("{chatId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IResult> DeleteChat(Guid chatId)
        {
            try
            {
                await ChatsService.DeleteChatAsync(chatId);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
            return Results.NoContent();
        }
    }
}
