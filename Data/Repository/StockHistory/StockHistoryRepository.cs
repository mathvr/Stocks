using Microsoft.EntityFrameworkCore;

namespace STOCKS.Data.Repository.StockHistory;

public class StockHistoryRepository : IStockHistoryRepository
{
    private readonly StocksContext _context;

    public StockHistoryRepository(StocksContext context)
    {
        _context = context;
    }

    public STOCKS.StockHistory GetByGuid(Guid guid)
    {
        return _context.Stockhistories
            .First(a => a.Id.Equals(guid));
    }

    public IQueryable<STOCKS.StockHistory> GetAsQueryable()
    {
        return _context.Stockhistories
            .AsQueryable();
    }

    public IQueryable<STOCKS.StockHistory> GetAsQueryableAsNoTracking()
    {
        return _context.Stockhistories
            .AsNoTracking()
            .AsQueryable();
    }

    public void Add(STOCKS.StockHistory stockhistory)
    {
        if (stockhistory == null)
        {
            throw new ArgumentNullException(nameof(stockhistory));
        }

        stockhistory.CreatedOn = DateTimeOffset.Now;
        stockhistory.CreatedBy = "Admin";
        stockhistory.ModifiedOn = null;
        stockhistory.ModifiedBy = null;

        _context.Stockhistories.Add(stockhistory);

        _context.SaveChanges();
    }

    public void Delete(Guid StockHistoryId)
    {
        var historyToDelete = _context.Appusers
            .FirstOrDefault(u => u.Id.Equals(StockHistoryId));

        if (historyToDelete == null)
        {
            throw new ArgumentNullException(nameof(historyToDelete));
        }

        _context.Appusers.Remove(historyToDelete);
    }

    public void Update(STOCKS.StockHistory stockHistory)
    {
        stockHistory.ModifiedOn = DateTimeOffset.Now;
        stockHistory.ModifiedBy = "Admin";
        
        _context.Stockhistories.Update(stockHistory);
        _context.SaveChanges();
    }
}