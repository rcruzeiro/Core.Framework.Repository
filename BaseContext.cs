using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Core.Framework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.Framework.Repository
{
    public abstract class BaseContext : DbContext, IUnitOfWorkAsync
    {
        IDbContextTransaction transaction;

        protected readonly string _connectionString;

        DbContext IUnitOfWork.Context => this;

        protected BaseContext()
        { }

        protected BaseContext(DbContextOptions options)
            : base(options)
        { }

        protected BaseContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected BaseContext(IDataSource source)
            : this(source.GetConnectionString())
        { }

        IEnumerable<T> IUnitOfWork.Get<T>(Func<T, bool> predicate)
        {
            try
            {
                return Set<T>()
                    .NullSafeWhere(predicate)
                    .ToList();
            }
            catch (Exception ex)
            { throw ex; }
        }

        IEnumerable<T> IUnitOfWork.Get<T>(ISpecification<T> spec)
        {
            try
            {
                var query = GetSpecIQueryable(spec);

                // return the result of the query using the specification's criteria expression
                return query
                    .NullSafeWhere(spec.Criteria)
                    .ToList();
            }
            catch (Exception ex)
            { throw ex; }
        }

        async Task<IEnumerable<T>> IUnitOfWorkAsync.GetAsync<T>(Func<T, bool> predicate, CancellationToken cancellationToken)
        {
            try
            {
                return await Task.FromResult(
                    Set<T>().NullSafeWhere(predicate)
                        .ToList());
            }
            catch (Exception ex)
            { throw ex; }
        }

        async Task<IEnumerable<T>> IUnitOfWorkAsync.GetAsync<T>(ISpecification<T> spec, CancellationToken cancellationToken)
        {
            try
            {
                var query = GetSpecIQueryable(spec);

                // return the awaitable result of the query using the specification's criteria expression
                return await query
                    .NullSafeWhere(spec.Criteria)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        T IUnitOfWork.Find<T>(params object[] keyValues)
        {
            try
            {
                return Find<T>(keyValues);
            }
            catch (Exception ex)
            { throw ex; }
        }

        async Task<T> IUnitOfWorkAsync.FindAsync<T>(object[] keyValues, CancellationToken cancellationToken)
        {
            try
            {
                return await FindAsync<T>(keyValues, cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        T IUnitOfWork.Add<T>(T entity)
        {
            try
            {
                return Add(entity).Entity;
            }
            catch (Exception ex)
            { throw ex; }
        }

        void IUnitOfWork.Add<T>(IEnumerable<T> entities)
        {
            try
            {
                AddRange(entities);
            }
            catch (Exception ex)
            { throw ex; }
        }

        async Task IUnitOfWorkAsync.AddAsync<T>(T entity, CancellationToken cancellationToken)
        {
            try
            {
                await AddAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        async Task IUnitOfWorkAsync.AddAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            try
            {
                await AddRangeAsync(entities, cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        T IUnitOfWork.Update<T>(T entity)
        {
            try
            {
                return Update(entity).Entity;
            }
            catch (Exception ex)
            { throw ex; }
        }

        void IUnitOfWork.Update<T>(IEnumerable<T> entities)
        {
            try
            {
                UpdateRange(entities);
            }
            catch (Exception ex)
            { throw ex; }
        }

        void IUnitOfWork.Remove<T>(T entity)
        {
            try
            {
                Remove(entity);
            }
            catch (Exception ex)
            { throw ex; }
        }

        void IUnitOfWork.Remove<T>(IEnumerable<T> entities)
        {
            try
            {
                RemoveRange(entities);
            }
            catch (Exception ex)
            { throw ex; }
        }

        int IUnitOfWork.SaveChanges()
        {
            try
            {
                return SaveChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }

        async Task<int> IUnitOfWorkAsync.SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        void IUnitOfWork.BeginTransaction()
        {
            transaction = Database.BeginTransaction();
        }

        void IUnitOfWork.Commit()
        {
            if (transaction != null)
                transaction.Commit();
        }

        void IUnitOfWork.Rollback()
        {
            if (transaction != null)
                transaction.Rollback();
        }

        public override void Dispose()
        {
            if (transaction != null)
                transaction.Dispose();

            base.Dispose();
        }

        private IQueryable<T> GetSpecIQueryable<T>(ISpecification<T> spec)
            where T : class, IAggregationRoot
        {
            // fetch a Queryable that includes all expression-based includes
            var includes = spec.Includes
                .Aggregate(Set<T>().AsQueryable(),
                (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var stringIncludes = spec.IncludeStrings
                .Aggregate(includes,
                (current, include) => current.Include(include));

            return stringIncludes;
        }
    }

    static class NullSafeExtensions
    {
        internal static IEnumerable<T> NullSafeWhere<T>(this IEnumerable<T> source, Func<T, bool> predicate = null)
        {
            return predicate == null ? source : source.Where(predicate);
        }

        internal static IQueryable<T> NullSafeWhere<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null ? source : source.Where(predicate).AsQueryable();
        }
    }
}
