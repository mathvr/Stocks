namespace STOCKS.Data.Repository.StockHistory;

public interface IStockHistoryRepository
{
    STOCKS.StockHistory GetByGuid(Guid guid);
    IQueryable<STOCKS.StockHistory> GetAsQueryable();
    IQueryable<STOCKS.StockHistory> GetAsQueryableAsNoTracking();
    void Add(STOCKS.StockHistory stockhistory);
    void Delete(Guid StockHistoryId);
    void Update(STOCKS.StockHistory stockHistory);
}