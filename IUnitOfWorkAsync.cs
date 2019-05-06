﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Framework.Entities;

namespace Core.Framework.Repository
{
    public interface IUnitOfWorkAsync : IUnitOfWork
    {
        Task<IEnumerable<T>> GetAsync<T>(Func<T, bool> predicate = null, CancellationToken cancellationToken = default)
            where T : class, IDbObject;

        Task<IEnumerable<T>> GetAsync<T>(ISpecification<T> spec, CancellationToken cancellationToken = default)
            where T : class, IDbObject;

        Task<T> FindAsync<T>(object[] keyValues, CancellationToken cancellationToken = default)
            where T : class, IDbObject;

        Task AddAsync<T>(T entity, CancellationToken cancellationToken = default)
            where T : class, IDbObject;

        Task AddAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default)
            where T : class, IDbObject;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}