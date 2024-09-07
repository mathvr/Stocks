namespace STOCKS.Data.Repository;

public partial interface IRepository<T> where T : class
{
    T GetByName(string name);
    T GetByGuid(Guid guid);
    IQueryable<T> GetAsQueryable();
    IQueryable<T> GetAsQueryableAsNoTracking();
    void Add(T entity);
    void Delete(T entity);
    void Delete(Guid guid);
    void Update(T entity);
    void Save();
}