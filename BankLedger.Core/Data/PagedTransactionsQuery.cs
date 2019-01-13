using BankLedger.Core.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankLedger.Core.Data
{
    public class PagedTransactionsQuery : IDatabaseQuery<IEnumerable<Transaction>>
    {
        public int AccountId { get; set; }

        public int Start { get; }

        public int RowCount { get; }

        public PagedTransactionsQuery(int accountId, int start, int rowCount)
        {
            AccountId = accountId;
            Start = start;
            RowCount = rowCount;
        }

        public async Task<IEnumerable<Transaction>> ExecuteAsync(SQLiteAsyncConnection db)
        {
            return await db.Table<Transaction>()
                           .Where(t => t.AccountId == AccountId)
                           .OrderByDescending(t => t.Timestamp)
                           .Skip(Start)
                           .Take(RowCount)
                           .ToListAsync();
        }
    }
}