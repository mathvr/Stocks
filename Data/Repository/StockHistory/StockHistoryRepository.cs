using Microsoft.EntityFrameworkCore;
using stocks.Data.DbContext;
using StockHistory = stocks.Data.Entities.StockHistory;

namespace STOCKS.Data.Repository.StockHistory;

public class StockHistoryRepository : IRepository<stocks.Data.Entities.StockHistory>
{
    private readonly StocksContext _context;

    public StockHistoryRepository(StocksContext context)
    {
        _context = context;
    }

    public stocks.Data.Entities.StockHistory GetByName(string name)
    {
        throw new NotImplementedException();
    }

    public stocks.Data.Entities.StockHistory GetByGuid(Guid guid)
    {
        return _context.Stockhistories
            .First(a => a.Id.Equals(guid));
    }

    public IQueryable<stocks.Data.Entities.StockHistory> GetAsQueryable()
    {
        return _context.Stockhistories
            .AsQueryable();
    }

    public IQueryable<stocks.Data.Entities.StockHistory> GetAsQueryableAsNoTracking()
    {
        return _context.Stockhistories
            .AsNoTracking()
            .AsQueryable();
    }

    public void Add(stocks.Data.Entities.StockHistory stockhistory)
    {
        if (stockhistory == null)
        {
            throw new ArgumentNullException(nameof(stockhistory));
        }

        stockhistory.CreatedOn = DateTimeOffset.Now;
        stockhistory.ModifiedOn = null;

        _context.Stockhistories.Add(stockhistory);
    }

    public void Delete(stocks.Data.Entities.StockHistory stockhistory)
    {
        _context.Stockhistories.Remove(stockhistory);
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

    public void Update(stocks.Data.Entities.StockHistory stockHistory)
    {
        stockHistory.ModifiedOn = DateTimeOffset.Now;
        
        _context.Stockhistories.Update(stockHistory);
        _context.SaveChanges();
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
    
    public void SqlQuery(string query)
    {
        _context.Database.ExecuteSqlRaw(query);
    }
}