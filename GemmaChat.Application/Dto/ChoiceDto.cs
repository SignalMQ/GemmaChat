using System.Text.Json.Serialization;

namespace GemmaChat.Application.Dto
{
    public class ChoiceDto
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }
        [JsonPropertyName("logprobs")]
        public object LogProbs { get; set; } = new object();
        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; } = string.Empty;
        [JsonPropertyName("message")]
        public ResponseMessageDto Message { get; set; } = new ResponseMessageDto();
    }
}