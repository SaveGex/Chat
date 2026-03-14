using Application.ModelsDTO;
using Application.Services.Interfaces;
using ChatApi.Hubs.Interfaces;
using Domain.Records;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private IChatsHub ChatsHub { get; init; }

        private IMessagesService MessagesService { get; init; }

        public MessagesController(IChatsHub chatsHub, IMessagesService messagesService)
        {
            ChatsHub = chatsHub;
            MessagesService = messagesService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(MessageResponseDTO), StatusCodes.Status200OK)]
        public async Task<IResult> CreateMessage(
            [FromBody] MessageCreateDTO dto)
        {
            MessageResponseDTO result;
            try
            {
                result = await MessagesService.CreateMessageAsync(dto);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
            await ChatsHub.SendMessageAsync(result);
            return Results.Created();
        }

        [HttpGet("{chatId:int}")]
        [ProducesResponseType(typeof(KeysetPaginationAfterResult<MessageResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<KeysetPaginationAfterResult<MessageResponseDTO>>> GetMessages(
            [FromQuery] string? after,
            [FromQuery] string? propName,
            [FromQuery] int? limit,
            [FromQuery] bool? reverse)
        {
            KeysetPaginationAfterResult<MessageResponseDTO> result;
            try
            {
                result = await MessagesService.GetMessagesKeysetPaginationAsync(after, propName, limit, reverse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(result);
        }

        [HttpDelete("{messageId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IResult> DeleteMessage(Guid messageId)
        {
            MessageResponseDTO result;
            try
            {
                result = await MessagesService.DeleteMessageAsync(messageId);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
            await ChatsHub.DeleteMessageAsync(result);
            return Results.NoContent();
        }

        [HttpPut("{messageId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IResult> UpdateMessage(
            Guid messageId,
            [FromBody] MessageUpdateDTO dto)
        {
            MessageResponseDTO result;
            try
            {
                result = await MessagesService.UpdateMessageAsync(messageId, dto);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
            await ChatsHub.UpdateMessageAsync(result);
            return Results.NoContent();
        }


    }
}
