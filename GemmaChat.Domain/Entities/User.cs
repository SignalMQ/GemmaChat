using System.ComponentModel.DataAnnotations;

namespace GemmaChat.Domain.Entities
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        [Key]
        public long ChatId { get; set; }
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastRequest { get; set; }
        public Conversation Conversation { get; set; } = null!;
    }
}
