using System;
using System.Collections.Generic;
using Core.Framework.Entities;

namespace Core.Framework.Repository
{
    public interface IRepository<T> : IDisposable
        where T : class, IDbObject
    {
        IEnumerable<T> Get(Func<T, bool> predicate = null);

        IEnumerable<T> Get(ISpecification<T> spec);

        T Find(params object[] keyValues);

        void Add(T entity);

        void Add(IEnumerable<T> entities);

        void Update(T entity);

        void Update(IEnumerable<T> entities);

        void Remove(T entity);

        void Remove(IEnumerable<T> entities);

        int SaveChanges();
    }
}
