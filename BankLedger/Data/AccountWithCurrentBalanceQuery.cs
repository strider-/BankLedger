using BankLedger.Core.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankLedger.Core.Data
{
    public class AccountWithCurrentBalanceQuery : IDatabaseQuery<IEnumerable<Account>>
    {
        public async Task<IEnumerable<Account>> ExecuteAsync(SQLiteAsyncConnection db)
        {
            return await db.QueryAsync<Account>(
                @"SELECT 
                    a.[Id], 
                    a.[Name],
                    a.[InitialBalance],
                    a.[InitialBalance] + IFNULL(SUM(t.[Amount]), 0) AS CurrentBalance
                FROM [Account] AS a
                LEFT JOIN [Transaction] AS t ON t.[AccountId] = a.Id
                GROUP BY a.[Id]
                ORDER BY a.[Id]");
        }
    }
}
