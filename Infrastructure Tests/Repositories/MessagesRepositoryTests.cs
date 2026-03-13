namespace Infrastructure_Tests.Repositories
{
    public class MessagesRepositoryTests
    {
        //private MessagesRepository CreateRepositoryWithSeededMessages(int totalMessages, out SchoolChatContext context)
        //{
        //    var options = new DbContextOptionsBuilder<SchoolChatContext>()
        //        .UseInMemoryDatabase("MessagesPaginationLarge_" + Guid.NewGuid())
        //        .Options;

        //    context = new TestSchoolChatContext(options);
        //    context.Database.EnsureCreated();

        //    for (int i = 1; i <= totalMessages; i++)
        //    {
        //        context.Messages.Add(new Message
        //        {
        //            Id = i,
        //            ChatId = 1,
        //            Author = $"Author{i}",
        //            Text = $"Message {i}",
        //            CreatedAt = DateTime.Now.AddMinutes(i)
        //        });
        //    }
        //    context.SaveChanges();

        //    IServiceProvider serviceProvider = new ServiceCollection()
        //        .AddPagination()
        //        .BuildServiceProvider();

        //    var paginationService = serviceProvider.GetRequiredService<IPaginationService>();

        //    return new MessagesRepository(context, paginationService);
        //}

        //[Fact]
        //public async Task GetMessagesKeysetPaginationAsync_ShouldPageThroughAllMessages()
        //{
        //    // Arrange
        //    int totalMessages = 50;
        //    int pageSize = 10;
        //    var repo = CreateRepositoryWithSeededMessages(totalMessages, out var context);

        //    string? after = null;
        //    int pagesCounted = 0;
        //    List<int> allIds = new();

        //    // Act
        //    do
        //    {
        //        var page = await repo.GetMessagesKeysetPaginationAsync(
        //            after: after,
        //            propName: nameof(Message.CreatedAt),
        //            limit: pageSize,
        //            IsDescending: true
        //        );

        //        if (!page.Data.Any())
        //            break;

        //        allIds.AddRange(page.Data.Select(m => m.Id));

        //        after = page.After;
        //        pagesCounted++;

        //    } while (!string.IsNullOrEmpty(after));

        //    // Assert
        //    pagesCounted.Should().Be(totalMessages / pageSize); // 50/10 = 5 pages
        //    allIds.Count.Should().Be(totalMessages);
        //    allIds.Should().OnlyHaveUniqueItems();
        //    allIds.Distinct().Count().Should().Be(totalMessages);
        //}

        //[Fact]
        //public async Task GetMessagesKeysetPaginationAsync_ShouldWorkWithReverse()
        //{
        //    // Arrange
        //    int totalMessages = 30;
        //    int pageSize = 7;
        //    var repo = CreateRepositoryWithSeededMessages(totalMessages, out var context);

        //    string? after = null;
        //    List<int> allIds = new();

        //    // Act
        //    do
        //    {
        //        var page = await repo.GetMessagesKeysetPaginationAsync(
        //            after: after,
        //            propName: nameof(Message.CreatedAt),
        //            limit: pageSize,
        //            IsDescending: false
        //        );

        //        allIds.AddRange(page.Data.Select(m => m.Id));
        //        after = page.After;

        //    } while (!string.IsNullOrEmpty(after));

        //    // Assert
        //    allIds.Should().OnlyHaveUniqueItems();
        //    allIds.Count.Should().Be(totalMessages);
        //}

    }
}
