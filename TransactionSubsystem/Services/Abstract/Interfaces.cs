using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionSubsystem.Entities;

namespace Interfaces
{
    public interface IAuthenticationService
    {
        User CurrentUser { get; }

        bool Login();
    }

    public interface IAmountVerificationService
    {
        bool VerifyAmount(User user, decimal amount);
    }

    public interface ITransactionCommitService
    {
        void CommitTransaction(Transaction transaction);
    }
}
