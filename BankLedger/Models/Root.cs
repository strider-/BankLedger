using SQLite;

namespace BankLedger.Core.Models
{
    public class Root
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}
