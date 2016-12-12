﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParrotWings.ViewModel
{
    public class TransactionViewModel
    {
        public int Id { get; set; }
        public string OwnerName { get; set; }
        public string CorrespondedUser { get; set; }
        public decimal Amount { get; set; }
        public decimal ResultingBalance { get; set; }
        public DateTime Date { get; set; }
        public bool Outgoing { get; set; }
    }
}