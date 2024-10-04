using Newtonsoft.Json;

namespace STOCKS.Models.ApiModels.OpenAi;

public class GetReputationBodyModel
{
    [JsonProperty("model")]
    public string Model { get; set; }
    
    [JsonProperty("messages")]
    public List<Message> Messages { get; set; }
    
    [JsonProperty("temperature")]
    public decimal Temperature { get; set; }

    public class Message
    {
        [JsonProperty("role")]
        public string Role { get; set; }
        
        [JsonProperty("content")]
        public string Content { get; set; }
    }
}