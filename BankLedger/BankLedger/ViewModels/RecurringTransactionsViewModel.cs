using BankLedger.Data;
using BankLedger.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BankLedger.ViewModels
{
    public class RecurringTransactionsViewModel : BaseViewModel
    {
        public ObservableCollection<RecurringTransactionSummary> Summaries { get; set; }

        public Command LoadSummariesCommand { get; set; }

        public Command DeleteRecurringTransactionCommand { get; set; }

        private IDatabaseQuery<IEnumerable<RecurringTransactionSummary>> Query { get; set; }

        public RecurringTransactionsViewModel()
        {
            Title = "Recurring Transactions";
            Query = new RecurringTransactionSummaryQuery();
            Summaries = new ObservableCollection<RecurringTransactionSummary>();

            LoadSummariesCommand = new Command(async () => await LoadData(LoadSummariesAsync));
            DeleteRecurringTransactionCommand = new Command((obj) => ConfirmDeletion(obj));

            MessagingCenter.Subscribe<NewRecurringTransactionViewModel, ModelAction<RecurringTransaction>>(this, Messages.Add, (obj, arg) =>
            {
                LoadSummariesCommand.Execute(null);
            });
        }

        public async Task DeleteAsync(RecurringTransaction transaction)
        {
            await Database.DeleteAsync(transaction);
            var item = Summaries.Single(s => s.Id == transaction.Id);
            Summaries.Remove(item);
            MessagingCenter.Send(this, Messages.Delete, new ModelAction<RecurringTransaction>(transaction, ActionType.Delete));
        }

        private void ConfirmDeletion(object obj)
        {
            if (obj is RecurringTransactionSummary transaction)
            {
                MessagingCenter.Send(this, Messages.Confirmation, new RecurringTransaction
                {
                    AccountId = transaction.AccountId,
                    Amount = transaction.Amount,
                    Day = transaction.Day,
                    Description = transaction.Description,
                    Id = transaction.Id
                });
            }
        }

        private async Task LoadSummariesAsync()
        {
            Summaries.Clear();
            var summaries = await Database.ExecuteAsync(Query);
            foreach (var summary in summaries)
            {
                Summaries.Add(summary);
            }
        }
    }
}