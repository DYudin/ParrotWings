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
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException((name));
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException((email));

            Name = name;
            Email = email;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
        public string HashedPassword { get; set; }

        [NotMapped]
        public Transaction PreparingTransaction { get; set; }

        public decimal CurrentBalance { get; set; }

        [NotMapped]
        public IEnumerable<Transaction> CommitedTransactions { get; set; }

        public void PrepareNewTransaction()
        {
            PreparingTransaction = new Transaction();
            PreparingTransaction.TransactionOwner = this;
            // todo
        }
    }
}
