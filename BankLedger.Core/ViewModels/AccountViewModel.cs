using BankLedger.Core.Data;
using BankLedger.Core.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BankLedger.Core.ViewModels
{
    public class AccountViewModel : BaseViewModel
    {
        const int PageSize = 25;

        public Account Item { get; set; }

        public ObservableCollection<Transaction> Transactions { get; set; }

        public Command LoadFirstPageCommand { get; set; }

        public Command LoadNextPageCommand { get; set; }

        public Command DeleteTransactionCommand { get; set; }

        private bool _isEmpty;
        public bool IsEmpty
        {
            get { return _isEmpty; }
            set { SetProperty(ref _isEmpty, value); }
        }

        private double _balance;
        public double CurrentBalance
        {
            get { return _balance; }
            set { SetProperty(ref _balance, value); }
        }

        private int Offset { get; set; }

        public AccountViewModel(Account item = null)
        {
            Item = item;
            Title = item.Name;
            CurrentBalance = item.CurrentBalance;

            Transactions = new ObservableCollection<Transaction>();
            Transactions.CollectionChanged += (s, e) => IsEmpty = !Transactions.Any();
            LoadFirstPageCommand = new Command(async () => await LoadData(LoadFirstPageAsync));
            LoadNextPageCommand = new Command(async () => await LoadData(LoadNextPageAsync));
            DeleteTransactionCommand = new Command((obj) => ConfirmDeletion(obj));

            MessagingCenter.Subscribe<NewTransactionViewModel, ModelAction<Transaction>>(this, Messages.Add, (obj, arg) =>
            {
                Transactions.Insert(0, arg.Item);
                CurrentBalance += arg.Item.Amount;
            });

            MessagingCenter.Subscribe<string, EmptyAction>(this, Messages.HardRefresh, (s, e) => LoadFirstPageCommand.Execute(null));
        }

        public async Task DeleteAsync(Transaction transaction)
        {
            await Database.DeleteAsync(transaction);
            Transactions.Remove(transaction);
            CurrentBalance -= transaction.Amount;
            MessagingCenter.Send(this, Messages.Delete, new ModelAction<Transaction>(transaction, ActionType.Delete));
        }

        private async Task LoadFirstPageAsync()
        {
            Transactions.Clear();
            Offset = 0;

            await LoadNextPageAsync();
        }

        private async Task LoadNextPageAsync()
        {
            var query = new PagedTransactionsQuery(Item.Id, Offset, PageSize);
            var accountTransactions = await Database.ExecuteAsync(query);

            foreach (var transaction in accountTransactions)
            {
                Transactions.Add(transaction);
            }

            Offset += PageSize;
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