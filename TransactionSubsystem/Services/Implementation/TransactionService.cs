using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using TransactionSubsystem.Entities;
using TransactionSubsystem.Repositories.Abstract;

namespace TransactionSubsystem.Services.Implementation
{
    public class TransactionService : ITransactionService
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

        public IEnumerable<Transaction> GetTransactionsByUserName(string userName)
        {
            var list = _transactionRepository.FindBy(x => x.TransactionOwner.Name == userName || x.Recepient.Name == userName);
            return list;
        }

        public void CommitTransaction(Transaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(("transaction"));

            // 0. prepare
            transaction.TransactionOwner.CurrentBalance -= transaction.Amount;
            transaction.Recepient.CurrentBalance += transaction.Amount;

            transaction.ResultingBalance = transaction.TransactionOwner.CurrentBalance;
            transaction.Date = DateTime.Now;

            // 1. withdraw from donor
            _userRepository.Edit(transaction.TransactionOwner);
            // 2. send to recepient
            _userRepository.Edit(transaction.Recepient);
            // 3. create transaction record
            _transactionRepository.Add(transaction);

            // 4. commit
            _userRepository.Commit();
            //_transactionRepository.Commit();
        }
    }
}
