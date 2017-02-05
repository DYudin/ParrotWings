using System;

namespace TransactionSubsystem.Infrastructure.UnitOfWork.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Commits all changes made on the unit of work.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rolls back all changes made on the unit of work.
        /// </summary>
        void Rollback();
    }
}
