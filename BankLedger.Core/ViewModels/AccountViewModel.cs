using BankLedger.Core.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BankLedger.Core.ViewModels
{
    public class AccountViewModel : BaseViewModel
    {
        public Account Item { get; set; }

        public ObservableCollection<Transaction> Transactions { get; set; }

        public Command LoadTransactionsCommand { get; set; }

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

        public AccountViewModel(Account item = null)
        {
            Item = item;
            Title = item.Name;
            CurrentBalance = item.CurrentBalance;

            Transactions = new ObservableCollection<Transaction>();
            Transactions.CollectionChanged += Refresh;
            LoadTransactionsCommand = new Command(async () => await LoadData(LoadTransactionsAsync));
            DeleteTransactionCommand = new Command((obj) => ConfirmDeletion(obj));

            MessagingCenter.Subscribe<NewTransactionViewModel, ModelAction<Transaction>>(this, Messages.Add, (obj, arg) =>
            {
                Transactions.Insert(0, arg.Item);
            });

            MessagingCenter.Subscribe<string, EmptyAction>(this, Messages.HardRefresh, (s, e) => LoadTransactionsCommand.Execute(null));
        }

        public async Task DeleteAsync(Transaction transaction)
        {
            await Database.DeleteAsync(transaction);
            Transactions.Remove(transaction);
            MessagingCenter.Send(this, Messages.Delete, new ModelAction<Transaction>(transaction, ActionType.Delete));
        }

        private async Task LoadTransactionsAsync()
        {
            Transactions.Clear();
            var accountTransactions = await Database.GetAllAsync<Transaction>(t => t.AccountId == Item.Id);
            foreach (var transaction in accountTransactions.OrderByDescending(t => t.Timestamp))
            {
                Transactions.Add(transaction);
            }
        }

        private void Refresh(object sender, NotifyCollectionChangedEventArgs e)
        {
            IsEmpty = !Transactions.Any();
            CurrentBalance = Transactions.Aggregate(Item.InitialBalance, (b, t) => b += t.Amount);
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