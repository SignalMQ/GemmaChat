using System.Text.Json.Serialization;

namespace GemmaChat.Application.Dto
{
    public class ContentDto
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty; // "text" или "image_url"

        [JsonPropertyName("text")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Text { get; set; }

        [JsonPropertyName("image_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ImageUrlDto? ImageUrl { get; set; }
    }
}
