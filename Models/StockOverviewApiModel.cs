using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace STOCKS.Models
{
    public class StockOverviewApiModel
    {
    [JsonProperty("Name")]
    public string Name { get; set; } = null!;

    [JsonProperty("CIK")]
    public int Cik { get; set; }

    [JsonProperty("Symbol")]
    public string Symbol { get; set; }

    [JsonProperty("Industry")]
    public string Industry { get; set; } = null!;

    [JsonProperty("Exchange")]
    public string Exchange { get; set; } = null!;

    [JsonProperty("Description")]
    public string? Description { get; set; }

    [JsonProperty("Currency")]
    public string Currency { get; set; } = null!;

    [JsonProperty("Country")]
    public string Country { get; set; } = null!;

    [JsonProperty("Sector")]
    public string Sector { get; set; } = null!;

    [JsonProperty("FiscalYearEnd")]
    public string Fiscalyearend { get; set; } = null!;

    [JsonProperty("MarketCapitalization")]
    public decimal? Marketcapitalization { get; set; }

    [JsonProperty("BookValue")]
    public decimal? Bookvalue { get; set; }

    [JsonProperty("ProfitMargin")]
    public decimal? Profitmargin { get; set; }

    [JsonProperty("LatestQuarter")]
    public DateTimeOffset? Latestquarted { get; set; }
    }
}