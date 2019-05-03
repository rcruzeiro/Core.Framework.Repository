using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.Framework.Repository
{
    public abstract class CoreContext : DbContext, IUnitOfWork
    {
        IDbContextTransaction transaction;

        protected CoreContext(DbContextOptions options)
            : base(options)
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
