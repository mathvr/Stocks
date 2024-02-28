namespace STOCKS.Data.Repository.StockOverview;

public interface IStockOverviewRepository
{
    STOCKS.StockOverview GetByNamel(string name);
    STOCKS.StockOverview GetByGuid(Guid guid);
    IQueryable<STOCKS.StockOverview> GetAsQueryable();
    IQueryable<STOCKS.StockOverview> GetAsQueryableAsNoTracking();
    void Add(STOCKS.StockOverview stockoverview);
    void Delete(string stockOverviewName);

    void Update(STOCKS.StockOverview stockoverview);
    void Save();
}
