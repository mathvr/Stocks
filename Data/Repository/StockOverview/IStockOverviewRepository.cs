namespace STOCKS.Data.Repository.StockOverview;

public interface IStockOverviewRepository
{
    stocks.Data.Entities.StockOverview GetByName(string name);
    stocks.Data.Entities.StockOverview GetByGuid(Guid guid);
    IQueryable<stocks.Data.Entities.StockOverview> GetAsQueryable();
    IQueryable<stocks.Data.Entities.StockOverview> GetAsQueryableAsNoTracking();
    void Add(stocks.Data.Entities.StockOverview stockoverview);
    void Delete(string stockOverviewName);

    void Update(stocks.Data.Entities.StockOverview stockoverview);
    void Save();
}
