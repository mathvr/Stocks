using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace STOCKS.Models
{
    public class StockOverviewApiModel
    {
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [JsonProperty("ticker")]
        public string Symbol { get; set; }
        
        [JsonProperty("exchangeCode")]
        public string Exchange { get; set; } = null!;

        [JsonProperty("description")]
        public string? Description { get; set; }
        
        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }
        
        [JsonProperty("endDate")]
        public DateTime EndDate { get; set; }
    }
}