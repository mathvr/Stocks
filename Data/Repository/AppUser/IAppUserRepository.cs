namespace STOCKS.Data.Repository.AppUser;

public interface IAppUserRepository
{
    Appuser GetByEmail(string email, bool expandStockOverviews);
    Appuser GetByGuid(Guid guid, bool expandStockOverviews);
    IQueryable<Appuser> GetAsQueryable(bool expandStockOverviews);
    IQueryable<Appuser> GetAsQueryableAsNoTracking();
    void Add(Appuser user);
    void Delete(string userEmail);
    void Update(Appuser user);
    void Save();
}