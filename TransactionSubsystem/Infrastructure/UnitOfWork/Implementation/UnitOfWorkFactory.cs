using System;
using System.Data.Entity;
using TransactionSubsystem.Infrastructure.UnitOfWork.Abstract;

namespace TransactionSubsystem.Infrastructure.UnitOfWork.Implementation
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly DbContext _dbContext;

        public UnitOfWorkFactory(DbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException(nameof(dbContext));

            _dbContext = dbContext;
        }

        public IUnitOfWork Create()
        {
            var result = new UnitOfWork(_dbContext);
            return result;
        }
    }
}
