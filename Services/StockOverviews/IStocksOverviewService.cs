using STOCKS.Models;

namespace STOCKS.Services.StockOverviews
{
    public interface IStocksOverviewService
    {
        ServiceResponse AddCompanyToUserProfile(string companySymbol);
        public ServiceResponse AddCompanyToSite(string companySymbol);
        ServiceResponse UpdateStockOverviews();
        List<StockOverviewDto> GetUserStockOverviews(string userEmail);
        TServiceResponse<List<StockOverviewDto>> GetExistingStockOverviews(string query, int top);
        ServiceResponse DeleteStockOverview(string symbol);
    }
}