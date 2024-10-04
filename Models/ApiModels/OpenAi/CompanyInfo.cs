using Newtonsoft.Json;

namespace STOCKS.Models.ApiModels.OpenAi;

public class CompanyInfo
{
    [JsonProperty("symbol")]
    public string Symbol { get; set; }
                
    [JsonProperty("reputation")]
    public int Reputation { get; set; }
                
    [JsonProperty("facts")]
    public List<string> Facts { get; set; }
}