using STOCKS.Models;

namespace STOCKS.Services.StockOverviews
{
    public interface IStocksOverviewService
    {
        ServiceResponse AddCompanyToUserProfile(string userEmail, string companySymbol);
        public ServiceResponse AddCompanyToSite(string companySymbol);
        ServiceResponse UpdateStockOverviews();
    }
}