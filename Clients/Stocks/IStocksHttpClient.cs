using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using stocks.Data.Entities;
using STOCKS.Models;

namespace STOCKS.Clients
{
    public interface IStocksHttpClient
    {
        StockOverviewApiModel GetCompanyOverview(string companySymbol);
        List<StockOverviewApiModel> GetCompanyOverviews(List<string> companySymbols);
        Task<StockOverviewandResponse> GetTimeSerie(StockOverview stockOverview, DateTime fromDate);
    }
}