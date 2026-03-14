namespace Application.ModelsDTO
{
    public class MessageUpdateDTO
    {
#warning Finish Message Update DTO
        public Guid Id { get; set; }
        public Guid ChatId { get; set; }
        public string Message { get; set; } = null!;
        public DateTime UpdatedTime { get; set; } = DateTime.UtcNow;
    }
}
