using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TransactionSubsystem.Entities;
using TransactionSubsystem.Repositories.Abstract;

namespace TransactionSubsystem.Repositories.Implementation
{
    public class TransactionRepository : EntityRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(TransactionSubsystemContext context)
            : base(context)
        {
        }
    }
}
