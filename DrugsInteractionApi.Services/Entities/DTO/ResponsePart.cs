
using Newtonsoft.Json;

namespace DrugsInteractionApi.Services.Entities.DTO
{
    public class ResponsePart
    {
        [JsonProperty("answer")]
        public InteractionResponse Answer { get; set; } = default!;
        [JsonProperty("source")]
        public string Source { get; set; } = string.Empty;
        [JsonProperty("language")]
        public string Language { get; set; } = string.Empty;
    }
}
