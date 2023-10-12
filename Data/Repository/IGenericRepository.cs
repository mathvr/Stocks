using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STOCKS.Data.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(object id);
        void Add(T obj);
        void Update (T obj);
        void Delete (T obj);
        string Save();
    }
}