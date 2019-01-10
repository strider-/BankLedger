using BankLedger.Core.Data;
using BankLedger.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BankLedger.Core.Services
{
    public interface IDatabase
    {
        Task<T> GetAsync<T>(int id) where T : Root, new();

        Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate = null) where T : Root, new();

        Task<int> SaveAsync<T>(T item) where T : Root, new();

        Task<int> DeleteAsync<T>(T item) where T : Root, new();

        Task<T> ExecuteAsync<T>(IDatabaseQuery<T> query);

        Task<int> ExecuteAsync(IDatabaseCommand command);
    }
}