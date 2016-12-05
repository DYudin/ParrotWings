using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TransactionSubsystem.Entities;
using TransactionSubsystem.Repositories.Abstract;

namespace Interfaces
{
    public interface IAuthenticationService
    {
        User CurrentUser { get; }

        bool Login(string userName, string password);
    }

    public interface IUserProvider
    {
        User CreateUser(string username, string email, string password, int[] roles);
        User GetUser(int userId);
    }



    public interface IAmountVerificationService
    {
        bool VerifyAmount(User user, decimal amount);
    }

    public interface ITransactionCommitService
    {
        void CommitTransaction(Transaction transaction);
    }

    public interface ISecurityService
    {
        string CreateSalt();
       
        string EncryptPassword(string password, string salt);
    }
}
