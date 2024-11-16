using Microsoft.EntityFrameworkCore;
using stocks.Data.DbContext;
using stocks.Data.Entities;

namespace STOCKS.Data.Repository.Splits;

public class SplitRepository : IRepository<Split>
{
    private readonly StocksContext _context;

    public SplitRepository(StocksContext context)
    {
        _context = context;
    }
    
    public Split GetByName(string name)
    {
        throw new NotImplementedException();
    }

    public Split GetByGuid(Guid guid)
    {
        return _context.Splits
            .First(s => s.Id == guid);
    }

    public IQueryable<Split> GetAsQueryable()
    {
        return _context.Splits
            .AsQueryable();
    }

    public IQueryable<Split> GetAsQueryableAsNoTracking()
    {
        return _context.Splits
            .AsQueryable()
            .AsNoTracking();
    }

    public void Add(Split entity)
    {
        _context.Splits.Add(entity);
    }

    public void Delete(Split entity)
    {
        _context.Splits.Remove(entity);
    }

    public void Delete(Guid guid)
    {
        _context.Splits.Remove(GetByGuid(guid));
    }

    public void Update(Split entity)
    {
        _context.Splits.Update(entity);
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