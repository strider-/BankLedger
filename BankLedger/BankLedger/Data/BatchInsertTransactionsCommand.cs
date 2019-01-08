using BankLedger.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankLedger.Data
{
    public class BatchInsertTransactionsCommand : IDatabaseCommand
    {
        private readonly IEnumerable<Transaction> _transactions;

        public BatchInsertTransactionsCommand(IEnumerable<Transaction> transactions) => _transactions = transactions;

        public async Task<int> ExecuteAsync(SQLiteAsyncConnection db)
        {
            return await db.InsertAllAsync(_transactions, runInTransaction: true);
        }
    }
}
