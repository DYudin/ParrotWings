using System;
using System.Data.Entity;
using TransactionSubsystem.Infrastructure.Repositories.Abstract;
using TransactionSubsystem.Infrastructure.UnitOfWork.Abstract;

namespace TransactionSubsystem.Infrastructure.UnitOfWork.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IUserRepository _userRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly DbContext _context;
        private bool disposed;

        public UnitOfWork(
            IUserRepository userRepository, 
            ITransactionRepository transactionRepository,
            DbContext context)
        {
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
            _context = context;
        }

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public IUserRepository Users
        {
            get
            {
                //if (drinkRepository == null)
                //    drinkRepository = new DrinkRepository(db);
                return _userRepository;
            }
        }

        public ITransactionRepository Transactions
        {
            get
            {
                //if (moneyRepository == null)
                //    moneyRepository = new MoneyRepository(db);
                return _transactionRepository;
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Rollback()
        {
            // todo
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
