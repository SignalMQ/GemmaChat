using System.Text.Json.Serialization;

namespace GemmaChat.Application.Dto
{
    public class Request
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;
        [JsonPropertyName("messages")]
        public List<MessageDto> Messages { get; set; } = [];
        [JsonPropertyName("temperature")]
        public double Temperature { get; set; }
        [JsonPropertyName("max_tokens")]
        public int MaxTokens { get; set; }
        [JsonPropertyName("stream")]
        public bool Stream { get; set; }
    }
}