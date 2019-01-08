using BankLedger.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankLedger.Data
{
    public class RecurringTransactionSummaryQuery : IDatabaseQuery<IEnumerable<RecurringTransactionSummary>>
    {
        public async Task<IEnumerable<RecurringTransactionSummary>> ExecuteAsync(SQLiteAsyncConnection db)
        {
            return await db.QueryAsync<RecurringTransactionSummary>(
                @"SELECT
	                [a].[Name] AS [AccountName],
	                [rt].*,
	                MAX([t].[Timestamp]) AS [LastTransactionDate]
                FROM [RecurringTransaction] AS [rt]
                JOIN [Account] AS [a] ON [a].[Id] = [rt].[AccountId]
                LEFT OUTER JOIN [Transaction] [t] ON [t].[RecurringTransactionId] = [rt].[Id]
                GROUP BY [rt].[Id]");
        }
    }
}
