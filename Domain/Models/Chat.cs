
using Domain.Models.Contracts;

namespace Domain.Models;

public class Chat : IIdentifiable
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    /// <summary>
    /// Permissions what users can do
    /// </summary>
    public virtual ICollection<ChatUserPermission> UsersPermissions { get; set; } = new List<ChatUserPermission>();
    /// <summary>
    /// Users of this chat\group
    /// </summary>
    public virtual ICollection<User> Users { get; set; } = new List<User>();
    /// <summary>
    /// Messages of this chat\group
    /// </summary>
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    /// <summary>
    /// Deleted messages of this chat\group
    /// </summary>
    public virtual ICollection<Message> DeletedMessages { get; set; } = new List<Message>();
}
