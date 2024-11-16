using Microsoft.EntityFrameworkCore;
using stocks.Data.DbContext;
using stocks.Data.Entities;

namespace STOCKS.Data.Repository.StockOverview;

public class StockOverviewRepository : IRepository<stocks.Data.Entities.StockOverview>
{
    private readonly StocksContext _context;

    public StockOverviewRepository(StocksContext context)
    {
        _context = context;
    }
    
    public stocks.Data.Entities.StockOverview GetByName(string name)
    {
        return _context.Stockoverviews
            .First(s => s.Name.Equals(name));
    }

    public stocks.Data.Entities.StockOverview GetByGuid(Guid guid)
    {
        return _context.Stockoverviews
            .First(s => s.Id.Equals(guid));
    }

    public IQueryable<stocks.Data.Entities.StockOverview> GetAsQueryable()
    {
        return _context.Stockoverviews
            .AsQueryable();
    }

    public IQueryable<stocks.Data.Entities.StockOverview> GetAsQueryableAsNoTracking()
    {
        return _context.Stockoverviews
            .AsQueryable()
            .AsNoTracking();
    }

    public void Add(stocks.Data.Entities.StockOverview stockoverview)
    {
        if (stockoverview == null)
        {
            throw new ArgumentNullException(nameof(stockoverview));
        }
        
        stockoverview.CreatedOn = DateTimeOffset.Now;
        stockoverview.ModifiedOn = null;

        _context.Stockoverviews.Add(stockoverview);

        _context.SaveChanges();
    }

    public void Delete(stocks.Data.Entities.StockOverview stockoverview)
    {
        _context.Stockoverviews.Remove(stockoverview);
    }

    public void Delete(Guid guid)
    {
        _context.Stockoverviews.Remove(GetByGuid(guid));
    }

    public void Delete(string stockOverviewName)
    {
        var stockToDelete = _context.Stockoverviews
            .FirstOrDefault(u => u.Name.Equals(stockOverviewName));

        if (stockToDelete == null)
        {
            throw new ArgumentNullException(nameof(stockToDelete));
        }

        _context.Stockoverviews.Remove(stockToDelete);
    }

    public void Update(stocks.Data.Entities.StockOverview stockoverview)
    {
        stockoverview.ModifiedOn = DateTimeOffset.Now;
        
        _context.Stockoverviews.Update(stockoverview);
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