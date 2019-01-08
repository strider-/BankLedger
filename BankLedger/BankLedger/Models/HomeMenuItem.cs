using System;

namespace BankLedger.Models
{
    public enum MenuItemType
    {
        RecurringTransactions = 1000
    }

    public class HomeMenuItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public Type TargetType { get; set; }

        public object Data { get; set; }
    }
}
