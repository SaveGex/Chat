using Domain.Models.Contracts;

namespace Domain.Models;

public class Message : IIdentifiable
{
    public Guid Id { get; set; }

    public Guid ChatId { get; set; }

    public string Author { get; set; } = null!;

    public string? Text { get; set; } = null!;
    // ToDo 
    public List<MessageAttachment> Attachments { get; set; } = new List<MessageAttachment>();

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DeletedAt { get; set; }

    public virtual Chat Chat { get; set; } = null!;
}
