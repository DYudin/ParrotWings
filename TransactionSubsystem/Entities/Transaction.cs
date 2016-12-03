using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionSubsystem.Entities
{
    public class Transaction
    {
        public int Id { get; set; }

        [NotMapped]
        public bool CommitAvailableState { get; set; }

        public decimal Amount { get; set; }
        public User TransactionOwner { get; set; }
        public User Recepient { get; set; }
        public DateTime Date { get; set; }
        public decimal ResultingBalance { get; set; }
    }
}
