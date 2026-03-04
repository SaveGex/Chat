

using Domain.Models;
using FluentAssertions;
using Infrastructure.DB;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MR.AspNetCore.Pagination;

namespace Infrastructure_Tests.Repositories
{
    public class ChatsRepositoryTests
    {
        [Fact]
        public async Task GetChatKeysetPaginationAsync_ShouldPageThroughAllChats()
        {
            //    // Arrange
            //    IServiceProvider serviceProvider = new ServiceCollection()
            //        .AddPagination()
            //        .BuildServiceProvider();

            //    var options = new DbContextOptionsBuilder<SchoolChatContext>()
            //        .UseInMemoryDatabase(databaseName: "ChatPaginationTest_" + Guid.NewGuid())
            //        .Options;

            //    using var context = new TestSchoolChatContext(options);
            //    context.Database.EnsureCreated();

            //    // Seed chats via DbContextExtensions (тепер додаємо 5 чатів для перевірки пагінації)
            //    context.SeedData();

            //    // додаємо додаткові чати для тесту
            //    for (int i = 0; i < 5; i++)
            //    {
            //        context.Chats.Add(new Chat { Title = $"Extra Chat {i + 1}" });
            //    }
            //    await context.SaveChangesAsync();

            //    var paginationService = serviceProvider.GetRequiredService<IPaginationService>();
            //    var repo = new ChatsQueryRepository(context, paginationService);

            //    string? after = null;
            //    int pageSize = 3;
            //    List<int> allIds = new();

            //    // Act
            //    do
            //    {
            //        var page = await repo.GetChatKeysetPaginationAsync(
            //            after: after,
            //            propName: nameof(Chat.Id),
            //            limit: pageSize,
            //            IsDescending: false
            //        );

            //        if (!page.Data.Any())
            //            break;

            //        allIds.AddRange(page.Data.Select(c => c.Id));
            //        after = page.After;

            //    } while (!string.IsNullOrEmpty(after));

            //    // Assert
            //    allIds.Should().OnlyHaveUniqueItems();
            //    allIds.Count.Should().Be(context.Chats.Count());
            //    allIds.Should().BeInAscendingOrder();
            //}
            throw new NotImplementedException(); // Models has been changed, so this test is not relevant anymore

        }
    }
}
