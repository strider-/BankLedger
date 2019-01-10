using SQLite;

namespace BankLedger.Core.Models
{
    public class RecurringTransaction : Root
    {
        [Indexed]
        public int AccountId { get; set; }

        public string Description { get; set; }

        public double Amount { get; set; }

        public int Day { get; set; }
    }
}