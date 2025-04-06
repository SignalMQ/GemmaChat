using System.Text.Json.Serialization;

namespace GemmaChat.Application.Dto
{
    public class ResponseMessageDto
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;
    }
}
