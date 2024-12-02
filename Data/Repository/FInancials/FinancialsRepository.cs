using Microsoft.EntityFrameworkCore;
using stocks.Data.DbContext;
using stocks.Data.Entities;

namespace STOCKS.Data.Repository.FInancials;

public class FinancialsRepository(StocksContext context) : IRepository<Financials>
{
    public Financials GetByName(string name)
    {
        return context.Financials.First(f => f.PropertyName == name);
    }

    public Financials GetByGuid(Guid guid)
    {
        return context.Financials.First(f => f.Id == guid);
    }

    public IQueryable<Financials> GetAsQueryable()
    {
        return context.Financials.AsQueryable();
    }

    public IQueryable<Financials> GetAsQueryableAsNoTracking()
    {
        return context.Financials.AsNoTracking().AsQueryable();
    }

    public void Add(Financials entity)
    {
        context.Add(entity);
    }

    public void Delete(Financials entity)
    {
        context.Remove(entity);
    }

    public void Delete(Guid guid)
    {
        context.Remove(GetByGuid(guid));
    }

    public void Update(Financials entity)
    {
        context.Update(entity);
    }

    public void Save()
    {
        context.SaveChanges();
    }

    public void Dispose()
    {
        context.Dispose();
    }

    public void SqlQuery(string query)
    {
        context.Database.ExecuteSqlRaw(query);
    }
}