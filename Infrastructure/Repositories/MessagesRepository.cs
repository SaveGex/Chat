using Domain.Interfaces;
using Domain.Models;
using Domain.Records;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using MR.AspNetCore.Pagination;

namespace Infrastructure.Repositories
{
    public class MessagesRepository : IMessagesRepository
    {
        private SchoolChatContext Context { get; init; }
        private IPaginationService PaginationService { get; init; }

        public MessagesRepository(SchoolChatContext schoolChatContext, IPaginationService paginationService)
        {
            Context = schoolChatContext;
            PaginationService = paginationService;
        }

        public async Task<Message> CreateMessageAsync(Message message)
        {
            var result = Context.Messages.Add(message).Entity;
            await Context.SaveChangesAsync();
            return result;
        }

        public async Task<Message> DeleteMessageAsync(Guid messageId)
        {
            var message = await Context.Messages.SingleAsync(m => m.Id == messageId);
            var result = Context.Messages.Remove(message).Entity;
            await Context.SaveChangesAsync();
            return result;
        }

        public async Task<Message?> GetMessageByIdAsync(Guid messageId)
        {
            var message = await Context.Messages.SingleAsync(m => m.Id == messageId);
            return message;
        }

        public async Task<KeysetPaginationAfterResult<Message>> GetMessagesKeysetPaginationAsync(string? after, string propName, int limit, bool IsDescending)
        {
            IQueryable<Message> query = Context.Messages;

            KeysetQueryModel cursor = new KeysetQueryModel()
            {
                After = after,
                Size = limit
            };

            var keySet = await PaginationService.KeysetPaginateAsync(
                query,
                KeysetPaginationBuilderService<Message>.CreateActionKeysetPaginationBuilder(
                    propName,
                    nameof(Message.Id),
                    IsDescending
                ),
                async id => await Context.Messages.FindAsync(int.Parse(id)),
                queryModel: cursor
            );

            string? newAfter = keySet.Data.LastOrDefault()?.Id.ToString();

            KeysetPaginationAfterResult<Message> afterResult = new(
                After: newAfter,
                Data: keySet.Data,
                TotalCount: keySet.TotalCount,
                PageSize: keySet.PageSize,
                HasPrevious: keySet.HasPrevious,
                HasNext: keySet.HasNext
            );


            return afterResult;
        }

        public Task<Message> UpdateMessageAsync(Guid messageId, Message message)
        {
            throw new NotImplementedException();
        }
    }
}
