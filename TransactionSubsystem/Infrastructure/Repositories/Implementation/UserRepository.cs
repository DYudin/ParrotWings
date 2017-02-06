
using TransactionSubsystem.Entities;
using TransactionSubsystem.Infrastructure.Repositories.Abstract;
using TransactionSubsystem.Repositories;

namespace TransactionSubsystem.Infrastructure.Repositories.Implementation
{
    public class UserRepository : EntityRepository<User>, IUserRepository
    {
        public UserRepository(TransactionSubsystemContext context)
            : base(context)
        {
        }
    }
}
