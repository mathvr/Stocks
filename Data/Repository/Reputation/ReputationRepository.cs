using Microsoft.EntityFrameworkCore;
using stocks.Data.DbContext;

namespace STOCKS.Data.Repository.Reputation;

public class ReputationRepository : IRepository<stocks.Data.Entities.Reputation>
{
    private readonly StocksContext _stocksContext;

    public ReputationRepository(StocksContext stocksContext)
    {
        _stocksContext = stocksContext;
    }
    
    public stocks.Data.Entities.Reputation GetByName(string name)
    {
        return _stocksContext.Reputations
            .First(r => r.Symbol == name);
    }

    public stocks.Data.Entities.Reputation GetByGuid(Guid guid)
    {
        return _stocksContext.Reputations
            .First(r => r.Id == guid);
    }

    public IQueryable<stocks.Data.Entities.Reputation> GetAsQueryable()
    {
        return _stocksContext.Reputations
            .AsQueryable();
    }

    public IQueryable<stocks.Data.Entities.Reputation> GetAsQueryableAsNoTracking()
    {
        return _stocksContext.Reputations
            .AsNoTracking()
            .AsQueryable();
    }

    public void Add(stocks.Data.Entities.Reputation entity)
    {
        _stocksContext.Reputations
            .Add(entity);
    }

    public void Delete(stocks.Data.Entities.Reputation entity)
    {
        _stocksContext.Reputations
            .Remove(entity);
    }

    public void Delete(Guid guid)
    {
        _stocksContext.Reputations
            .Remove(GetByGuid(guid));
    }

    public void Update(stocks.Data.Entities.Reputation entity)
    {
        _stocksContext.Reputations
            .Update(entity);
    }

    public void Save()
    {
        _stocksContext
            .SaveChanges();
    }
}