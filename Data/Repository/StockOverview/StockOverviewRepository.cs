using Microsoft.EntityFrameworkCore;

namespace STOCKS.Data.Repository.StockOverview;

public class StockOverviewRepository : IStockOverviewRepository
{
    private readonly StocksContext _context;

    public StockOverviewRepository(StocksContext context)
    {
        _context = context;
    }
    
    public STOCKS.StockOverview GetByNamel(string name)
    {
        return _context.Stockoverviews
            .First(s => s.Name.Equals(name));
    }

    public STOCKS.StockOverview GetByGuid(Guid guid)
    {
        return _context.Stockoverviews
            .First(s => s.Id.Equals(guid));
    }

    public IQueryable<STOCKS.StockOverview> GetAsQueryable()
    {
        return _context.Stockoverviews
            .AsQueryable();
    }

    public IQueryable<STOCKS.StockOverview> GetAsQueryableAsNoTracking()
    {
        return _context.Stockoverviews
            .AsQueryable()
            .AsNoTracking();
    }

    public void Add(STOCKS.StockOverview stockoverview)
    {
        if (stockoverview == null)
        {
            throw new ArgumentNullException(nameof(stockoverview));
        }
        
        stockoverview.CreatedOn = DateTimeOffset.Now;
        stockoverview.CreatedBy = "Admin";
        stockoverview.ModifiedOn = null;
        stockoverview.ModifiedBy = null;

        _context.Stockoverviews.Add(stockoverview);

        _context.SaveChanges();
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

    public void Update(STOCKS.StockOverview stockoverview)
    {
        stockoverview.ModifiedBy = "Admin";
        stockoverview.ModifiedOn = DateTimeOffset.Now;
        
        _context.Stockoverviews.Update(stockoverview);
        _context.SaveChanges();
    }
}