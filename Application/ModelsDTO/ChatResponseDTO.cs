
using Domain.Models;

namespace Application.ModelsDTO;

public class ChatResponseDTO
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    public virtual ICollection<ChatUserPermission> UsersPermissions { get; set; } = new List<ChatUserPermission>();
}
