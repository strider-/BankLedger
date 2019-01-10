using SQLite;
using System;

namespace BankLedger.Core.Models
{
    public class Transaction : Root
    {
        [Indexed]
        public int AccountId { get; set; }

        public string Description { get; set; }

        public double Amount { get; set; }

        public DateTime Timestamp { get; set; }

        [Indexed]
        public int? RecurringTransactionId { get; set; }
    }
}