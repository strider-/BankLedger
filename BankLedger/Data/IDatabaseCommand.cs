using SQLite;
using System.Threading.Tasks;

namespace BankLedger.Core.Data
{
    public interface IDatabaseCommand
    {
        Task<int> ExecuteAsync(SQLiteAsyncConnection db);
    }
}
