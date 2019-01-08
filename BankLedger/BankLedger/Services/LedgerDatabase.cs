using BankLedger.Data;
using BankLedger.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BankLedger.Services
{
    public class LedgerDatabase : IDatabase
    {
        private SQLiteAsyncConnection _db;

        public LedgerDatabase()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ledger.db3");

            _db = new SQLiteAsyncConnection(dbPath);
            _db.CreateTablesAsync<Account, Transaction, RecurringTransaction>().GetAwaiter().GetResult();
        }

        public async Task<T> GetAsync<T>(int id) where T : Root, new()
        {
            return await _db.GetAsync<T>(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate = null) where T : Root, new()
        {
            return await _db.Table<T>().Where(predicate ?? (_ => true)).ToListAsync().ConfigureAwait(false);
        }

        public async Task<int> SaveAsync<T>(T item) where T : Root, new()
        {
            if (item.Id == 0)
            {
                return await _db.InsertAsync(item).ConfigureAwait(false);
            }
            else
            {
                return await _db.UpdateAsync(item).ConfigureAwait(false);
            }
        }

        public async Task<int> DeleteAsync<T>(T item) where T : Root, new()
        {
            return await _db.DeleteAsync(item).ConfigureAwait(false);
        }

        public async Task<T> ExecuteAsync<T>(IDatabaseQuery<T> query)
        {
            return await query.ExecuteAsync(_db).ConfigureAwait(false);
        }

        public async Task<int> ExecuteAsync(IDatabaseCommand command)
        {
            return await command.ExecuteAsync(_db).ConfigureAwait(false);
        }
    }
}