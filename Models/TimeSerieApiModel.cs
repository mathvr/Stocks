using Newtonsoft.Json;

namespace STOCKS.Models;

public class TimeSerieApiModel
{
    [JsonProperty("Time Series (60min)")]
    public Dictionary<DateTime, TimeSerieUnit> TimeSerie { get; set; }
    
    [JsonProperty("Meta Data")]
    public MetaData Meta { get; set; }

    public class TimeSerieUnit
    {
        [JsonProperty("1. open")]
        public decimal Open { get; set; }
        [JsonProperty("2. high")]
        public decimal High { get; set; }
        [JsonProperty("3. low")]
        public decimal Low { get; set; }
        [JsonProperty("4. close")]
        public decimal Close { get; set; }
        [JsonProperty("5. volume")]
        public int Volume { get; set; }
    }

    public class MetaData
    {
        [JsonProperty("1. Information")]
        public string Information { get; set;}
        [JsonProperty("2. Symbol")]
        public string Symbol { get; set;}
        [JsonProperty("3. Last Refreshed")]
        public DateTime LastRefresh { get; set;}
        [JsonProperty("4. Interval")]
        public string Interval { get; set;}
        [JsonProperty("5. Output Size")]
        public string OutputSize { get; set;}
        [JsonProperty("6. Time Zone")]
        public string TimeZone { get; set;}
    }
}