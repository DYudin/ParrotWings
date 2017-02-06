using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ParrotWings.ViewModel
{
    public class TransactionViewModel
    {
        public int Id { get; set; }
        public string OwnerName { get; set; }
        [Required]
        public string CorrespondedUser { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public decimal ResultingBalance { get; set; }
        public DateTime Date { get; set; }
        public bool Outgoing { get; set; }
    }
}