using Domain.Models;
using Infrastructure_Tests;
using System.Data;

namespace Infrastructure.DB
{
    public static class DbContextExtensions
    {
        public static void SeedData(this TestSchoolChatContext context, int messagesCount = 50)
        {
            //if (context.Users.Any() || context.Chats.Any())
            //    return;

            //// Users
            //var users = new[]
            //{
            //    new User { Username = "admin_user", AzureAdObjectId = Guid.NewGuid().ToString(), Identifier = Guid.NewGuid().ToString()},
            //    new User { Username = "teacher_smith", AzureAdObjectId = Guid.NewGuid().ToString(), Identifier = Guid.NewGuid().ToString()},
            //    new User { Username = "student_john", AzureAdObjectId = Guid.NewGuid().ToString(), Identifier = Guid.NewGuid().ToString()}
            //};
            //context.Users.AddRange(users);
            //context.SaveChanges();

            //// Chats
            //var chats = new[]
            //{
            //    new Chat { Title = "General School Chat" },
            //    new Chat { Title = "Math Class 10-A" }
            //};
            //context.Chats.AddRange(chats);
            //context.SaveChanges();

            //var mathChatId = chats.Single(c => c.Title == "Math Class 10-A").Id;

            //// Messages - seed with specified number of messages
            //var messages = Enumerable.Range(1, messagesCount)
            //    .Select(i => new Message
            //    {
            //        ChatId = mathChatId,
            //        Author = i % 2 == 0 ? "teacher_smith" : "student_john",
            //        Text = $"Message {i}",
            //        CreatedAt = DateTime.Now.AddMinutes(i)
            //    })
            //    .ToList();

            //context.Messages.AddRange(messages);
            //context.SaveChanges();

            //// Permissions and ChatUserPermissions
            //var permissions = context.Permissions.ToList();
            //if (permissions.Any())
            //{
            //    var chatPermissions = new[]
            //    {
            //        new ChatUserPermission
            //        {
            //            ChatId = mathChatId,
            //            UserId = users.Single(u => u.Username == "teacher_smith").Id,
            //            PermissionId = permissions.Single(p => p.Name == "ReadMessages").Id
            //        }
            //    };
            //    context.ChatUserPermissions.AddRange(chatPermissions);
            //    context.SaveChanges();
            //}
            throw new NotImplementedException(); // user model changed, need to update seeding logic
        }

    }
}
