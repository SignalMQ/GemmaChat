using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GemmaChat.Domain.Entities
{
    public class Content
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(Message))]
        public long MessageId { get; set; }
        public string Type { get; set; } = null!;
        public string? Text { get; set; }
        public ImageUrl? ImageUrl { get; set; }
        public DateTime Created { get; set; }
    }
}
