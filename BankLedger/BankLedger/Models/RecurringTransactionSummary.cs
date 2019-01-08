using BankLedger.Extensions;
using System;

namespace BankLedger.Models
{
    public class RecurringTransactionSummary
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string AccountName { get; set; }

        public string Description { get; set; }

        public double Amount { get; set; }

        public int Day { get; set; }

        public DateTime? LastTransactionDate { get; set; }

        public string Label => LastTransactionDate.HasValue
            ? $"{AccountName}, on the {Day.Place()} ({LastTransactionDate.Value.ToShortDateString()})"
            : $"{AccountName}, on the {Day.Place()}";
    }
}
