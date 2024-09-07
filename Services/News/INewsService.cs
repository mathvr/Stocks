using STOCKS.Models;

namespace stocks.Services.News;

public interface INewsService
{
    ServiceResponse AddArticles(int fromDays);
    TServiceResponse<List<ArticleDto>> GetByStockOverview(string symbol, int fromDays);
}