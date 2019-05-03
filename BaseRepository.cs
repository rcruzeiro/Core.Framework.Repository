using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Framework.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Framework.Repository
{
    public abstract class BaseRepository<T> : IRepositoryAsync<T>
        where T : class, IDbObject
    {
        protected readonly DbContext _context;

        protected BaseRepository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<T> Get(Func<T, bool> predicate = null)
        {
            try
            {
                return _context.Set<T>()
                    .Where(predicate)
                    .ToList();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public IEnumerable<T> Get(ISpecification<T> spec)
        {
            try
            {
                var query = GetSpecIQueryable(spec);

                // return the result of the query using the specification's criteria expression
                return query
                    .Where(spec.Criteria)
                    .ToList();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<IEnumerable<T>> GetAsync(Func<T, bool> predicate = null, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Set<T>()
                    .Where(predicate)
                    .AsQueryable()
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<IEnumerable<T>> GetAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            try
            {
                var query = GetSpecIQueryable(spec);

                // return the awaitable result of the query using the specification's criteria expression
                return await query
                    .Where(spec.Criteria)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public T Find(params object[] keyValues)
        {
            try
            {
                return _context.Find<T>(keyValues);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<T> FindAsync(object[] keyValues, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.FindAsync<T>(keyValues, cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Add(T entity)
        {
            try
            {
                _context.Add(entity);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Add(IEnumerable<T> entities)
        {
            try
            {
                _context.AddRange(entities);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.AddAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task AddAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.AddRangeAsync(entities, cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Update(T entity)
        {
            try
            {
                _context.Update(entity);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Update(IEnumerable<T> entities)
        {
            try
            {
                _context.UpdateRange(entities);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Remove(T entity)
        {
            try
            {
                _context.Remove(entity);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Remove(IEnumerable<T> entities)
        {
            try
            {
                _context.RemoveRange(entities);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public int SaveChanges()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                    _context.Dispose();
            }
        }

        private IQueryable<T> GetSpecIQueryable(ISpecification<T> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var includes = spec.Includes
                .Aggregate(_context.Set<T>().AsQueryable(),
                (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var stringIncludes = spec.IncludeStrings
                .Aggregate(includes,
                (current, include) => current.Include(include));

            return stringIncludes;
        }
    }
}