using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace STOCKS.Models;

public class TimeSerieApiModels
{
    public List<TimeSerieApiModel> Timeseries { get; set; }
}
public class TimeSerieApiModel
{
    [JsonProperty("date")]
    public DateTime Date { get; set; }
    
    [JsonProperty("open")]
    public Decimal OpenValue { get; set; }
    
    [JsonProperty("close")]
    public Decimal CloseValue { get; set; }
    
    [JsonProperty("high")]
    public Decimal High { get; set; }
    
    [JsonProperty("low")]
    public Decimal Low { get; set; }
    
    [JsonProperty("volume")]
    public int Volume { get; set; }
    
    [JsonProperty("divCash")]
    public Decimal Dividend { get; set; }
    
    [JsonProperty("splitFactor")]
    public Decimal Split { get; set; }
}