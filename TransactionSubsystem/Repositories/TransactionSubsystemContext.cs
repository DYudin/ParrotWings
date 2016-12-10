using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionSubsystem.Entities;

namespace TransactionSubsystem.Repositories
{
    public class TransactionSubsystemContext : DbContext
    {
        public TransactionSubsystemContext()
            : base("TransactionSystemStore")
        {
            Initialize();
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }

        private void Initialize()
        {
            InitializeUsers();
            InitializeTransactions();
        }

        private void InitializeUsers()
        {
            if (!Users.Any())
            {
                Users.Add(new User("John Smith", "hh@mail.ru") {CurrentBalance = 500});
                Users.Add(new User("Margaret Dobi", "DobiM@mail.ru") {CurrentBalance = 500});
                Users.Add(new User("Dmitriy Yudin", "exdv@mail.ru") {CurrentBalance = 500});

                SaveChanges();
            }
        }
        private void InitializeTransactions()
        {
            if (!Transactions.Any())
            {
                var user1 = new User("John", "John2");
                var user2 = new User("Ann", "ANn2");
                var user3 = new User("dyudin", "Ajya");

                Transactions.Add(new Transaction() { Recepient = user1, TransactionOwner = user3, Amount = 200, ResultingBalance = 300, Date = DateTime.Now });
                Transactions.Add(new Transaction() { Recepient = user2, TransactionOwner = user3, Amount = 100, ResultingBalance = 250, Date = DateTime.Now });
                Transactions.Add(new Transaction() { Recepient = user3, TransactionOwner = user2, Amount = 50, ResultingBalance = 150, Date = DateTime.Now });

                //Transactions.Add(new Transaction("Dmitriy Yudin", "exdv@mail.ru") { CurrentBalance = 500 });

                SaveChanges();
            }
        }
    }
}
