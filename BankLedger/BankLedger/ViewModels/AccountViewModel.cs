using BankLedger.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BankLedger.ViewModels
{
    public class AccountViewModel : BaseViewModel
    {
        public Account Item { get; set; }

        public ObservableCollection<Transaction> Transactions { get; set; }

        public Command LoadTransactionsCommand { get; set; }

        public Command DeleteTransactionCommand { get; set; }

        public AccountViewModel(Account item = null)
        {
            Title = item?.Name;
            Item = item;

            Transactions = new ObservableCollection<Transaction>();
            LoadTransactionsCommand = new Command(async () => await LoadTransactionsAsync());
            DeleteTransactionCommand = new Command((obj) => ConfirmDeletion(obj));

            MessagingCenter.Subscribe<NewTransactionViewModel, ModelAction<Transaction>>(this, Messages.Add, (obj, arg) =>
            {
                Transactions.Insert(0, arg.Item);
            });
        }

        public async Task DeleteAsync(Transaction transaction)
        {
            await Database.DeleteAsync(transaction);
            Transactions.Remove(transaction);
            MessagingCenter.Send(this, Messages.Delete, new ModelAction<Transaction>(transaction, ActionType.Delete));
        }

        private async Task LoadTransactionsAsync()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                await Task.Delay(100);
                Transactions.Clear();
                var accountTransactions = await Database.GetAllAsync<Transaction>(t => t.AccountId == Item.Id);
                foreach (var transaction in accountTransactions.OrderByDescending(t => t.Timestamp))
                {
                    Transactions.Add(transaction);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ConfirmDeletion(object obj)
        {
            if (obj is Transaction transaction)
            {
                MessagingCenter.Send(this, Messages.Confirmation, transaction);
            }
        }
    }
}