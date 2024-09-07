using Microsoft.EntityFrameworkCore;
using stocks.Data.DbContext;

namespace STOCKS.Data.Repository.AppUser;

public class AppUserRepository : IAppUserRepository
{
    private readonly StocksContext _context;

    public AppUserRepository(StocksContext context)
    {
        _context = context;
    }
    
    public Appuser GetByEmail(string email, bool expandStockOverviews)
    {
        var appUsers = _context.Appusers;

        return expandStockOverviews
            ? appUsers
                .First(a => a.Email.Equals(email))
            : appUsers
                .First(a => a.Email.Equals(email));
    }

    public Appuser GetByGuid(Guid guid, bool expandStockOverviews)
    {
        var appUsers = _context.Appusers;

        return expandStockOverviews
            ? appUsers
                .Include(a => a.Stockoverviews)
                .FirstOrDefault(a => a.Id.Equals(guid)) ?? null
            : appUsers
                .FirstOrDefault(a => a.Id.Equals(guid)) ?? null;
    }

    public IQueryable<Appuser> GetAsQueryable(bool expandStockOverviews)
    {
        var appUsers = _context.Appusers;

        return expandStockOverviews
            ? appUsers
                .AsQueryable()
            : appUsers
                .AsQueryable();
    }

    public IQueryable<Appuser> GetAsQueryableAsNoTracking()
    {
        return _context.Appusers
            .AsNoTracking()
            .AsQueryable();
    }

    public void Add(Appuser user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        
        user.CreatedOn = DateTimeOffset.Now;
        user.CreatedBy = "Admin";
        user.ModifiedOn = null;
        user.ModifiedBy = null;

        _context.Appusers.Add(user);

        _context.SaveChanges();
    }

    public void Delete(string userEmail)
    {
        var userToDelete = _context.Appusers
            .FirstOrDefault(u => u.Password.Equals(userEmail));

        if (userToDelete == null)
        {
            throw new ArgumentNullException(nameof(userToDelete));
        }

        _context.Appusers.Remove(userToDelete);
    }

    public void Update(Appuser user)
    {
        user.ModifiedBy = "Admin";
        user.ModifiedOn = DateTimeOffset.Now;
        
        _context.Appusers.Update(user);
        _context.SaveChanges();
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}