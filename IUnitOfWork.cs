using System;
using System.Collections.Generic;
using Core.Framework.Entities;

namespace Core.Framework.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IEnumerable<T> Get<T>(Func<T, bool> predicate = null)
            where T : class, IDbObject;

        IEnumerable<T> Get<T>(ISpecification<T> spec)
            where T : class, IDbObject;

        T Find<T>(params object[] keyValues)
            where T : class, IDbObject;

        void Add<T>(T entity)
            where T : class, IDbObject;

        void Add<T>(IEnumerable<T> entities)
            where T : class, IDbObject;

        void Update<T>(T entity)
            where T : class, IDbObject;

        void Update<T>(IEnumerable<T> entities)
            where T : class, IDbObject;

        void Remove<T>(T entity)
            where T : class, IDbObject;

        void Remove<T>(IEnumerable<T> entities)
            where T : class, IDbObject;

        int SaveChanges();

        void BeginTransaction();

        void Commit();

        void Rollback();
    }
}
