using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GemmaChat.Domain.Entities
{
    public class ImageUrl
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(Content))]
        public long ContentId { get; set; }
        public string Url { get; set; } = null!;
    }
}
