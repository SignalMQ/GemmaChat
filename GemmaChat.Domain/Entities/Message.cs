using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GemmaChat.Domain.Entities
{
    public class Message
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(Conversation))]
        public long ConversationId { get; set; }
        public string Role { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime Created { get; set; }
    }
}
