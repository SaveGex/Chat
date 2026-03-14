namespace Application.ModelsDTO
{
    public class MessageCreateDTO
    {
        public Guid ChatId { get; set; }

        public string Author { get; set; } = null!;

        public string Text { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
