﻿using System;
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
            if (!Users.Any())
            {
                var user1 = new User("John Smith", "hh@mail.ru");
                user1.FillSecurityProperties("6nE3MGQbNRfyvnQ72cE0xQ==", "LuHEjGKKpu541fUyKiv5QQFFY7j8cu35bJ2Zcf65PZI=");

                var user2 = new User("Margaret Dobi", "DobiM@mail.ru");
                user2.FillSecurityProperties("kgdvYJ29VhEVxccnQo3mDw==", "HLwIG/CeMgR5QqTxODnYuLxlhEyVmKvyO+d5Qqho0uw=");


                var user3 = new User("Dmitriy Yudin", "exdv@mail.ru");
                user3.FillSecurityProperties("7acTAIx7GlqtnRwbWnSQeQ==", "/C9lLTUlSnOhLwA5RThR/YMyQDaXtXdCU5pw/zjDaGU=");

                Users.Add(user1);
                Users.Add(user2);
                Users.Add(user3);

                //Transactions.Add(new Transaction() {
                //    Recepient = user1, TransactionOwner = user3, Amount = 200, OwnerResultingBalance = 300, RecepientResultingBalance=700, Date = DateTime.Now });
                //Transactions.Add(new Transaction() {
                //    Recepient = user2, TransactionOwner = user3, Amount = 100, OwnerResultingBalance = 200, RecepientResultingBalance = 600, Date = DateTime.Now });
                //Transactions.Add(new Transaction() {
                //    Recepient = user3, TransactionOwner = user2, Amount = 50, OwnerResultingBalance = 250,
                //    RecepientResultingBalance = 550,
                //    Date = DateTime.Now });                 

                SaveChanges();
            }
        }    
    }
}
