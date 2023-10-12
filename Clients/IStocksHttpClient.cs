using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using STOCKS.Models;

namespace STOCKS.Clients
{
    public interface IStocksHttpClient
    {
        StockOverviewApiModel GetCompanyOverview(string companySymbol);
        List<StockOverviewApiModel> GetCompanyOverviews(List<string> companySymbols);
    }
}