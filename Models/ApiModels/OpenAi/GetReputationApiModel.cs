using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace STOCKS.Models.ApiModels.OpenAi;

public class GetReputationApiModel
{
    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("choices")]
    public List<Choice> Choices { get; set; }

    public class Choice
    {
        [JsonProperty("index")]
        public int Index { get; set; }
        
        [JsonProperty("message")]
        public MainMessage Message { get; set; }

        public class MainMessage
        {
            [JsonProperty("role")]
            public string Role { get; set; }
            
            [JsonProperty("content")]
            public string Content { get; set; }

            public CompanyInfo CompanyReputation { get; set; } = null;
        }
    }
}