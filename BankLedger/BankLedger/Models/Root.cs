using SQLite;

namespace BankLedger.Models
{
    public class Root
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}
