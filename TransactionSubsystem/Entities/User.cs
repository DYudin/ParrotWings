using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransactionSubsystem.Entities
{
    public class User
    {
        public User()
        {
        }
        public User(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("name");
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("email");

            Name = name;
            Email = email;
            CurrentBalance = 500;
        }

        public int Id { get; set; }
        public string Name { get; protected set; }
        public string Email { get; protected set; }
        public string Salt { get; protected set; }
        public string HashedPassword { get; protected set; }

        //[NotMapped]
        //public Transaction PreparingTransaction { get; set; }

        public decimal CurrentBalance { get; protected set; }

        [NotMapped]
        public IEnumerable<Transaction> Transactions { get; protected set; }

        //public void PrepareNewTransaction()
        //{
        //    PreparingTransaction = new Transaction();
        //    PreparingTransaction.TransactionOwner = this;
        //    // todo
        //}

        public void ExecuteTransaction(Transaction transaction)
        {
            //todo
            if (transaction == null) throw new ArgumentNullException("Transaction");
            if (transaction.Amount <= 0) throw new ArgumentException("Transaction amount must be greater than 0");
           
            // todo
            transaction.TransactionOwner = this;

            if (transaction.TransactionOwner.Name == transaction.Recepient.Name) throw new ArgumentException("Recipient must be different from the transaction sender");
            if (transaction.Amount > transaction.TransactionOwner.CurrentBalance) throw new ArgumentException("Not enough money");

            transaction.TransactionOwner.CurrentBalance -= transaction.Amount;
            transaction.Recepient.CurrentBalance += transaction.Amount;

            transaction.OwnerResultingBalance = transaction.TransactionOwner.CurrentBalance;
            transaction.RecepientResultingBalance = transaction.Recepient.CurrentBalance;
        }

        public void FillSecurityProperties(string passwordSalt, string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(passwordSalt)) throw new ArgumentException("passwordSalt");
            if (string.IsNullOrWhiteSpace(hashedPassword)) throw new ArgumentException("hashedPassword");

            Salt = passwordSalt;
            HashedPassword = hashedPassword;
        }
    }
}
