using Newtonsoft.Json;

namespace DrugsInteractionApi.Services.Entities.DTO
{
    public class InteractionResponse
    {
        [JsonProperty("Article")]
        public string Article { get; set; } = string.Empty;

        [JsonProperty("Drug1Name")]
        public string Drug1Name { get; set; } = string.Empty;

        [JsonProperty("Drug2Name")]
        public string Drug2Name { get; set; } = string.Empty;
    }
}
