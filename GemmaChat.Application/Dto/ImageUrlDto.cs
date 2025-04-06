using System.Text.Json.Serialization;

namespace GemmaChat.Application.Dto
{
    public class ImageUrlDto
    {
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }
}
