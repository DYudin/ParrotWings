
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionSubsystem.Entities;

namespace Interfaces
{
    public interface IAuthenticationService
    {
        User CurrentUser { get; set; }

        Task<bool> Login(string email, string password);
    }

    public interface IUserProvider
    {
        Task<User> CreateUser(string username, string email, string password);
        User GetUser(int userId);
        User GetUserByName(string userName);
        Task<IEnumerable<User>> GetUsers();
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
