using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GemmaChat.Domain.Entities
{
    public class Conversation
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(User))]
        public long UserId { get; set; }
        public string Model { get; set; } = null!;
        public double Temperature { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public List<Message> Messages { get; set; } = null!;
    }
}
