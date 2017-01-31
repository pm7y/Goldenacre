// ReSharper disable CheckNamespace

namespace Goldenacre.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
    }

    /// <summary>
    ///     A repository pattern interface which includes key.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface IRepository<TDomainObject, in TPrimaryKey>
        where TDomainObject : class where TPrimaryKey : struct
    {
        IQueryable<TDomainObject> GetItems();

        IQueryable<TDomainObject> GetItems(Expression<Func<TDomainObject, bool>> filter);

        TDomainObject GetItem(TPrimaryKey key);

        TDomainObject GetItem(Expression<Func<TDomainObject, bool>> filter);

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