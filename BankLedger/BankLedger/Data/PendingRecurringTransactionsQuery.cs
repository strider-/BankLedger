using BankLedger.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankLedger.Data
{
    public class PendingRecurringTransactionsQuery : IDatabaseQuery<IEnumerable<Transaction>>
    {
        public async Task<IEnumerable<Transaction>> ExecuteAsync(SQLiteAsyncConnection db)
        {
            return await db.QueryAsync<Transaction>(
                @"SELECT
	                [rt].[AccountId],
	                [rt].[Description],
	                [rt].[Amount],
	                ((STRFTIME('%s', 'now') + 62135596800) * 10000000) AS [Timestamp],
	                [rt].[Id] AS [RecurringTransactionId],
	                DATETIME(([t].[Timestamp]/10000000) - 62135596800, 'unixepoch') as [dt]
                FROM [RecurringTransaction] AS [rt]
                LEFT OUTER JOIN [Transaction] AS [t] on 
	                [t].[RecurringTransactionId] = [rt].[Id] AND
	                [dt] BETWEEN 
		                DATE('now', 'start of month', CAST([rt].[Day] - 1 as text) || ' days') AND
		                DATE('now', 'start of month', '1 months', '-1 days')
                WHERE [dt] IS NULL AND CAST(STRFTIME('%d') as int) >= [rt].[Day]");
        }
    }
}
