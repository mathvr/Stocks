namespace STOCKS.Data.Repository.Connection;

public interface IConnectionRepository
{
    STOCKS.Connection GetByName(string name);
    STOCKS.Connection GetByGuid(Guid guid);
    IQueryable<STOCKS.Connection> GetAsQueryable();
    IQueryable<STOCKS.Connection> GetAsQueryableAsNoTracking();
    void Add(STOCKS.Connection connection);
    void Delete(string connectionName);
    void Update(STOCKS.Connection connection);
}