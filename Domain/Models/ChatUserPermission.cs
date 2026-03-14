
using Microsoft.EntityFrameworkCore;

namespace Domain.Models
{
    [PrimaryKey(nameof(Id))]
    public class ChatUserPermission
    {
        public Guid Id { get; set; }
        public Guid PermissionId { get; set; }

        public Guid ChatId { get; set; }
        public Chat Chat { get; set; } = null!;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public Permission Permission { get; set; } = null!;
    }
}
