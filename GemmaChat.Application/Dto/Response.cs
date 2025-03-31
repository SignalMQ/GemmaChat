using System.Text.Json.Serialization;

namespace GemmaChat.Application.Dto
{
    public class Response
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("object")]
        public string Object { get; set; } = string.Empty;
        [JsonPropertyName("created")]
        public int Created { get; set; }
        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;
        [JsonPropertyName("choices")]
        public List<ChoiceDto> Choices { get; set; } = [];
        [JsonPropertyName("usage")]
        public UsageDto Usage { get; set; } = new UsageDto();
        [JsonPropertyName("stats")]
        public object Stats { get; set; } = new object();
        [JsonPropertyName("system_fingerprint")]
        public string SystemFingerprint { get; set; } = string.Empty;
    }
}