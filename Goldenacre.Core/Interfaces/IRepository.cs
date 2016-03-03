using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Goldenacre.Core.Interfaces
{
    public interface IRepository<T, TKey> : IDisposable where T : class
    {
        IQueryable<T> Get(
            Expression<Func<T, bool>> filter = null);

        T Get(TKey key);
        void Insert(T entity);
        void Update(T entity);
        void Delete(TKey key);
        void Delete(T entity);
        void Save();
    }

    public interface IRepository<T> : IDisposable where T : class
    {
        IQueryable<T> Get(
            Expression<Func<T, bool>> filter = null);

        T Get(params object[] keys);
        void Insert(T entity);
        void Update(T entity);
        void Delete(params object[] keys);
        void Delete(T entity);
        void Save();
    }
}