using STOCKS.Models;

namespace stocks.Services.News;

public interface INewsService
{
    ServiceResponse AddArticles(int fromDays);
    TServiceResponse<IList<ArticlesGroup>> GetByStockOverview(string symbol, int fromDays, int perSymbol);
    public ServiceResponse DeleteAllArticles();
}