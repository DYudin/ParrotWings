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
    }
}
