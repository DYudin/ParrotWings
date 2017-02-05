
using TransactionSubsystem.Entities;

namespace TransactionSubsystem.Repositories.Abstract
{
    public interface IUserRepository : IEntityRepository<User> { }
      
    public interface ITransactionRepository : IEntityRepository<Transaction> { }
}
