using System;
using Interfaces;
using TransactionSubsystem.Entities;
using TransactionSubsystem.Repositories.Abstract;

namespace TransactionSubsystem.Services.Implementation
{
    public class TransactionCommitService : ITransactionCommitService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITransactionRepository _transactionRepository;
        public TransactionCommitService(IUserRepository userRepository, ITransactionRepository transactionRepository)
        {
            if (userRepository == null) throw new ArgumentNullException(nameof(userRepository));
            if (transactionRepository == null) throw new ArgumentNullException(nameof(transactionRepository));

            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
        }

        public void CommitTransaction(Transaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

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
