using SQLite;
using System.Threading.Tasks;

namespace BankLedger.Data
{
    public interface IDatabaseCommand
    {
        Task<int> ExecuteAsync(SQLiteAsyncConnection db);
    }
}
