namespace BankLedger.Models
{
    public class Account : Root
    {
        public string Name { get; set; }

        public double InitialBalance { get; set; }

        public double CurrentBalance { get; set; }
    }
}
