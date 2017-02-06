
using TransactionSubsystem.Entities;

namespace TransactionSubsystem.Infrastructure.Repositories.Abstract

{
    public interface IUserRepository : IEntityRepository<User> { }
      
    public interface ITransactionRepository : IEntityRepository<Transaction> { }
}
