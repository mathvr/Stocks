using stocks.Data.Entities;

namespace STOCKS.Models;

public class StockOverviewandResponse
{
    public HttpResponseMessage? Response { get; set; }
    public StockOverview StockOverview { get; set; }
}