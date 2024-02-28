using Microsoft.EntityFrameworkCore;

namespace STOCKS.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public StocksContext _context;
        private DbSet<T> _table;

        public Repository (StocksContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return _table.AsQueryable();
        }

            public T GetById(object id)
        {
            try
            {
                return _table.Find(id);
            }
            catch (Exception e)
            {
                throw new Exception($"The required object could be retrieved in the Db, see exception details: {e.Message}");
            }
        }

        public void Add(T obj)
        {
            try
            {
                _table.Add(obj);
            }
            catch (Exception e)
            {
                throw new Exception($"The required object could be added to the Db, see exception details: {e.Message}");
            }
        }

        public void Delete(T obj)
        {
            try
            {
                T objFromDb = _table.Find(obj);
                _table.Remove(objFromDb);
            }
            catch (Exception e)
            {
                throw new Exception($"The required object could be Deleted from Database, see exception details: {e.Message}");
            }
            
        }

        public string Save()
        {
            try
            {
                _context.SaveChanges();
                return "Saved changes successfully";
            }
            catch (Exception e)
            {
                return $"Error while trying to save DbContext, see exception details: {e.Message}"; 
            }
        }

        public void Update(T obj)
        {
            try
            {
                _table.Attach(obj);
                _context.Entry(obj).State = EntityState.Modified;
            }
            catch (Exception e)
            {
                throw new Exception($"The required object could be Updated in Database, see exception details: {e.Message}");
            }
        }
    }
}