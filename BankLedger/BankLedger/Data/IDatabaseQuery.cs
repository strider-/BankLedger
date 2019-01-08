using SQLite;
using System.Threading.Tasks;

namespace BankLedger.Data
{
    public interface IDatabaseQuery<T>
    {
        Task<T> ExecuteAsync(SQLiteAsyncConnection db);
    }
}
