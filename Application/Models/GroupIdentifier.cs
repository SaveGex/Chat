
namespace Application.Models
{
    /// <summary>
    /// Consists of 2 members: <see cref="ChatTitle"/> + '_' + <see cref="ChatId"/>
    /// <br/>Returns unique identifier: <b>ChatTitle_123</b>
    /// </summary>
    public class GroupIdentifier
    {
        public string ChatTitle { get; set; }
        public Guid ChatId { get; set; }

        public GroupIdentifier(string title, Guid id)
        {
            ChatTitle = title;
            ChatId = id;
        }

        public override string ToString()
        {
            return ChatTitle + '_' + ChatId;
        }
    }
}
