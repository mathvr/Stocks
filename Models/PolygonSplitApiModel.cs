using Newtonsoft.Json;

namespace STOCKS.Models;

public class PolygonSplitApiModel
{
    [JsonProperty("results")]
    public List<Split> Results { get; set; }
    
    [JsonProperty("status")]
    public string Status { get; set; }
    
    [JsonProperty("request_id")]
    public string RequestId { get; set; }
    
    [JsonProperty("next_url")]
    public string nextUrl { get; set; }
    public class Split
    {
        [JsonProperty("execution_date")]
        public DateTime ExecutionDate { get; set; }
        
        [JsonProperty("split_from")]
        public decimal SplitFrom { get; set; }
        
        [JsonProperty("split_to")]
        public decimal SplitTo { get; set; }
        
        [JsonProperty("ticker")]
        public string Ticker { get; set; }
        
        [JsonProperty("id")]
        public string SplitApiId { get; set; }
    }
}