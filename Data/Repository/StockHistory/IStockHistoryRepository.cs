namespace STOCKS.Data.Repository.StockHistory;

public interface IStockHistoryRepository
{
    stocks.Data.Entities.StockHistory GetByGuid(Guid guid);
    IQueryable<stocks.Data.Entities.StockHistory> GetAsQueryable();
    IQueryable<stocks.Data.Entities.StockHistory> GetAsQueryableAsNoTracking();
    void Add(stocks.Data.Entities.StockHistory stockhistory);
    void Delete(Guid StockHistoryId);
    void Update(stocks.Data.Entities.StockHistory stockHistory);
    void Save();
}