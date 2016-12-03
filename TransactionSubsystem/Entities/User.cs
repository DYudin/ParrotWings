using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionSubsystem.Entities
{
    public class User
    {
        public User()
        {
        }
        public User(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name));
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException(nameof(email));

            Name = name;
            Email = email;
        }

        public int Id { get; set; }

        public string Name { get; private set; }

        public string Email { get; private set; }

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
