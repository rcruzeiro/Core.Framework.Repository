using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Framework.Entities;

namespace Core.Framework.Repository
{
    public abstract class BaseRepository<T> : IRepositoryAsync<T>
        where T : class, IAggregationRoot
    {
        protected readonly IUnitOfWorkAsync _unitOfWork;

        protected BaseRepository(IUnitOfWorkAsync unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public virtual IEnumerable<T> Get(Func<T, bool> predicate = null)
        {
            try
            {
                return _unitOfWork.Get(predicate);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual IEnumerable<T> Get(ISpecification<T> spec)
        {
            try
            {
                return _unitOfWork.Get(spec);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual async Task<IEnumerable<T>> GetAsync(Func<T, bool> predicate = null, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _unitOfWork.GetAsync(predicate, cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual async Task<IEnumerable<T>> GetAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _unitOfWork.GetAsync(spec, cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual T Find(params object[] keyValues)
        {
            try
            {
                return _unitOfWork.Find<T>(keyValues);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual async Task<T> FindAsync(object[] keyValues, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _unitOfWork.FindAsync<T>(keyValues, cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual T Add(T entity)
        {
            try
            {
                return _unitOfWork.Add(entity);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual void Add(IEnumerable<T> entities)
        {
            try
            {
                _unitOfWork.Add(entities);
            }
            catch (Exception ex)
            { throw ex; }

        }

        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            try
            {
                await _unitOfWork.AddAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual async Task AddAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            try
            {
                await _unitOfWork.AddAsync(entities, cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual T Update(T entity)
        {
            try
            {
                return _unitOfWork.Update(entity);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual void Update(IEnumerable<T> entities)
        {
            try
            {
                _unitOfWork.Update(entities);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual void Remove(T entity)
        {
            try
            {
                _unitOfWork.Remove(entity);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual void Remove(IEnumerable<T> entities)
        {
            try
            {
                _unitOfWork.Remove(entities);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual int SaveChanges()
        {
            try
            {
                return _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void BeginTransaction()
        {
            try
            {
                _unitOfWork.BeginTransaction();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Commit()
        {
            try
            {
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Rollback()
        {
            try
            {
                _unitOfWork.Rollback();
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
                _unitOfWork.Dispose();
            }
        }
    }
}
