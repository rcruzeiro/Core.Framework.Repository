using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.Framework.Repository
{
    public abstract class BaseContext : DbContext, IUnitOfWork
    {
        IDbContextTransaction transaction;

        protected readonly string _connectionString;

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

        public void BeginTransaction()
        {
            transaction = Database.BeginTransaction();
        }

        public void Commit()
        {
            if (transaction != null)
                transaction.Commit();
        }

        public void Rollback()
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
    }
}
