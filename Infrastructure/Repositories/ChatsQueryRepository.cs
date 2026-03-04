using Domain.Interfaces;
using Domain.Models;
using Domain.Records;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using MR.AspNetCore.Pagination;

namespace Infrastructure.Repositories
{
    public class ChatsQueryRepository : IChatsQueryRepository
    {
        private SchoolChatContext Context { get; init; }
        private IPaginationService PaginationService { get; init; }

        public ChatsQueryRepository(SchoolChatContext schoolChatContext, IPaginationService paginationService)
        {
            Context = schoolChatContext;
            PaginationService = paginationService;
        }
        public async Task<IEnumerable<Chat>> ChatsContainsTheUser(Guid userId)
        {
            List<Chat> result = await Context.Chats
                .Where(c => c.Users.Any(u => u.Id == userId))
                .ToListAsync();
            return result;
        }

        public async Task<KeysetPaginationAfterResult<Chat>> GetChatKeysetPaginationAsync(string? after, string propName, int limit, bool IsDescending)
        {
            IQueryable<Chat> query = Context.Chats;

            KeysetQueryModel cursor = new KeysetQueryModel()
            {
                After = after,
                Size = limit
            };

            var keySet = await PaginationService.KeysetPaginateAsync(
                query,
                KeysetPaginationBuilderService<Chat>.CreateActionKeysetPaginationBuilder(
                    propName,
                    nameof(Chat.Id),
                    IsDescending
                ),
                async id => await Context.Chats.FindAsync(int.Parse(id)),
                queryModel: cursor
            );

            string? newAfter = keySet.Data.LastOrDefault()?.Id.ToString();

            KeysetPaginationAfterResult<Chat> afterResult = new(
                After: newAfter,
                Data: keySet.Data,
                TotalCount: keySet.TotalCount,
                PageSize: keySet.PageSize,
                HasPrevious: keySet.HasPrevious,
                HasNext: keySet.HasNext
            );


            return afterResult;
        }





    }
}
