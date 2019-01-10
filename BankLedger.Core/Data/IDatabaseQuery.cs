using SQLite;
using System.Threading.Tasks;

namespace BankLedger.Core.Data
{
    public interface IDatabaseQuery<T>
    {
        Task<T> ExecuteAsync(SQLiteAsyncConnection db);
    }
}
