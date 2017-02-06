
using TransactionSubsystem.Entities;
using TransactionSubsystem.Infrastructure.Repositories.Abstract;
using TransactionSubsystem.Repositories;

namespace TransactionSubsystem.Infrastructure.Repositories.Implementation
{
    public class TransactionRepository : EntityRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(TransactionSubsystemContext context)
            : base(context)
        {
        }
    }
}
