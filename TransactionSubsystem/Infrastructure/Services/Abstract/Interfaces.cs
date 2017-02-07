
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionSubsystem.Entities;

namespace TransactionSubsystem.Infrastructure.Services.Abstract
{
    public interface IAuthenticationService
    {
        //User CurrentUser { get; set; }

        string Login(string email, string password);
        void LogOut();
    }

    public interface IUserProvider
    {
        User CreateUser(string username, string email, string password);
        User GetUser(int userId);
        User GetUserByName(string userName);
        IEnumerable<User> GetUsers();
    }    

    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetTransactionsByUserName(string userName);
        Task CommitTransaction(Transaction transaction);
    }

    public interface ISecurityService
    {
        string CreateSalt();
       
        string EncryptPassword(string password, string salt);
    }
}
