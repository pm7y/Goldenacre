using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Goldenacre.Core.Interfaces
{
    /// <summary>
    ///     A repository interface which includes key.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IRepository<TDomainObject, TPrimaryKey>
        where TDomainObject : class
        where TPrimaryKey : struct
    {
        IQueryable<TDomainObject> Get(Expression<Func<TDomainObject, bool>> filter = null);
        TDomainObject GetOne(TPrimaryKey key);

        void Insert(TDomainObject entity);
        void Insert(IEnumerable<TDomainObject> entity);

        void Update(TDomainObject entity);
        void Update(IEnumerable<TDomainObject> entity);

        void Delete(TPrimaryKey key);
        void Delete(TDomainObject entity);
        void Delete(IEnumerable<TPrimaryKey> keys);
        void Delete(IEnumerable<TDomainObject> entity);
    }
}