using Microsoft.EntityFrameworkCore;
using stocks.Data.DbContext;
using stocks.Data.Entities;

namespace STOCKS.Data.Repository.News;

public class NewsRepository : IRepository<Article>
{
    private readonly StocksContext _context;

    public NewsRepository(StocksContext context)
    {
        _context = context;
    }
    public Article GetByName(string name)
    {
        return _context.Articles
            .First(a => a.Title == name);
    }

    public Article GetByGuid(Guid guid)
    {
        return _context.Articles
            .First(a => a.Id == guid);
    }

    public IQueryable<Article> GetAsQueryable()
    {
        return _context.Articles
            .AsQueryable();
    }

    public IQueryable<Article> GetAsQueryableAsNoTracking()
    {
        return _context.Articles
            .AsNoTracking()
            .AsQueryable();
    }

    public void Add(Article entity)
    {
        _context.Articles.Add(entity);
    }

    public void Delete(Article entity)
    {
        _context.Articles.Remove(entity);
    }

    public void Delete(Guid guid)
    {
        _context.Articles.Remove(GetByGuid(guid));
    }

    public void Update(Article entity)
    {
        _context.Articles.Update(entity);
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