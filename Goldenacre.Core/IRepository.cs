using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Goldenacre.Core
{
    public interface IRepository<T> : IDisposable where T : class
    {
        IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

        T Get(params object[] keys);
        void Insert(T entity);
        void Update(T entity);
        void Delete(params object[] keys);
        void Delete(T entity);
        void Save();
    }
}