﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParrotWings.ViewModel
{
    public class TransactionViewModel
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public string RecepientId { get; set; }
        public decimal Amount { get; set; }
        public decimal ResultingBalance { get; set; }
        public DateTime DateCommited { get; set; }
    }
}