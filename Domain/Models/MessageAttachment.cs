namespace Domain.Models
{
    public class MessageAttachment
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// FK key to the message
        /// </summary>
        public Guid MessageId { get; set; }
        /// <summary>
        /// Configuration property
        /// </summary>
        public Message Message { get; set; } = null!;
        /// <summary>
        /// Original full file name before coding it into byte array
        /// <br/> <b>photo.jpg</b> or <b>documents.docx</b>
        /// </summary>
        public string FileName { get; set; } = null!;
        /// <summary>
        /// image/jpeg
        /// </summary>
        public string ContentType { get; set; } = null!;
        /// <summary>
        /// For validating purposes
        /// </summary>
        public long FileSize { get; set; }
        /// <summary>
        /// Literally data
        /// </summary>
        public byte[] Data { get; set; } = null!;
    }
}