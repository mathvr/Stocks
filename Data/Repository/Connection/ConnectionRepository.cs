using Microsoft.EntityFrameworkCore;

namespace STOCKS.Data.Repository.Connection;

public class ConnectionRepository : IConnectionRepository
{
    private readonly StocksContext _context;

    public ConnectionRepository(StocksContext context)
    {
        _context = context;
    }
    /**/
    public STOCKS.Connection GetByName(string name)
    {
        return _context.Connections
            .First(c => c.Name.Equals(name));
    }

    public STOCKS.Connection GetByGuid(Guid guid)
    {
        return _context.Connections
            .First(a => a.Id.Equals(guid));
    }

    public IQueryable<STOCKS.Connection> GetAsQueryable()
    {
        return _context.Connections
            .AsQueryable();
    }

    public IQueryable<STOCKS.Connection> GetAsQueryableAsNoTracking()
    {
        return _context.Connections
            .AsNoTracking()
            .AsQueryable();
    }

    public void Add(STOCKS.Connection connection)
    {
        if (connection == null)
        {
            throw new ArgumentNullException(nameof(connection));
        }
        
        connection.CreatedOn = DateTimeOffset.Now;
        connection.CreatedBy = "Admin";
        connection.ModifiedOn = null;
        connection.ModifiedBy = null;

        _context.Connections.Add(connection);

        _context.SaveChanges();
    }

    public void Delete(string connectionName)
    {
        var connectionToDelete = _context.Connections
            .FirstOrDefault(u => u.Name.Equals(connectionName));

        if (connectionToDelete == null)
        {
            throw new ArgumentNullException(nameof(connectionToDelete));
        }

        _context.Connections.Remove(connectionToDelete);
    }

    public void Update(STOCKS.Connection connection)
    {
        connection.ModifiedBy = "Admin";
        connection.ModifiedOn = DateTimeOffset.Now;
        
        _context.Connections.Update(connection);
        _context.SaveChanges();
    }
}