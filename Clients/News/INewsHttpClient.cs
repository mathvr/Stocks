using stocks.Data.Entities;
using STOCKS.Models;

namespace stocks.Clients.News;

public interface INewsHttpClient
{
    Task<StockOverviewandResponse> GetNewsForTitleAsync(int daysAgo, StockOverview stockOverview);
}