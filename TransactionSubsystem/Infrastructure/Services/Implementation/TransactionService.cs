using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using TransactionSubsystem.Entities;
using TransactionSubsystem.Repositories.Abstract;
using System.Threading.Tasks;

namespace TransactionSubsystem.Services.Implementation
{
    public class TransactionService : ITransactionService //, IDisposable
    {
        private readonly IUserRepository _userRepository;
        private readonly ITransactionRepository _transactionRepository;
        public TransactionService(IUserRepository userRepository, ITransactionRepository transactionRepository)
        {
            if (userRepository == null) throw new ArgumentNullException(("userRepository"));
            if (transactionRepository == null) throw new ArgumentNullException(("transactionRepository"));

            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
        }

        // todo вынести в контроллер
        public Task<IEnumerable<Transaction>> GetTransactionsByUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentException(userName);

            return Task.Run(() => _transactionRepository.FindByIncluding(x => x.TransactionOwner.Name == userName || x.Recepient.Name == userName).AsEnumerable());
        }

        // todo вынести в unit of work и класс transaction. unit of work в контроллере
        public Task CommitTransaction(Transaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException("Transaction");
            if (transaction.Amount <= 0 ) throw new ArgumentException("Transaction amount must be greater than 0");
            if (transaction.TransactionOwner.Name == transaction.Recepient.Name) throw new ArgumentException("Recipient must be different from the transaction sender");
            if (transaction.Amount > transaction.TransactionOwner.CurrentBalance) throw new ArgumentException("Not enough money");

            return Task.Run(() => CommitTransactionInternal(transaction));
        }

        private void CommitTransactionInternal(Transaction transaction)
        {      
            //// 0. prepare
            //transaction.TransactionOwner.CurrentBalance -= transaction.Amount;
            //transaction.Recepient.CurrentBalance += transaction.Amount;

            //transaction.OwnerResultingBalance = transaction.TransactionOwner.CurrentBalance;
            //transaction.RecepientResultingBalance = transaction.Recepient.CurrentBalance;            

            //// 1. withdraw from donor
            //_userRepository.Edit(transaction.TransactionOwner);
            //// 2. send to recepient
            //_userRepository.Edit(transaction.Recepient);
            //// 3. create transaction record
            //_transactionRepository.Add(transaction);

            //// 4. commit
            //_userRepository.Commit();          
        }

        //protected override void Dispose(bool disposing)
        //{
        //    unitOfWork.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}
