using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Framework.Entities;

namespace Core.Framework.Repository
{
    public interface IUnitOfWorkAsync : IUnitOfWork
    {
        Task<IEnumerable<T>> GetAsync<T>(ISpecification<T> spec = null, CancellationToken cancellationToken = default)
            where T : class, IAggregationRoot;

        Task<T> FindAsync<T>(object[] keyValues, CancellationToken cancellationToken = default)
            where T : class, IAggregationRoot;

        Task AddAsync<T>(T entity, CancellationToken cancellationToken = default)
            where T : class, IAggregationRoot;

        Task AddAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default)
            where T : class, IAggregationRoot;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
