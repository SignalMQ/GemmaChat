using System.Text.Json.Serialization;

namespace GemmaChat.Application.Dto
{
    public class MessageDto
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;
    }
}