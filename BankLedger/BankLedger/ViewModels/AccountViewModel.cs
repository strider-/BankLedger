using BankLedger.Models;
using System.Collections.ObjectModel;
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

        private bool _isEmpty;
        public bool IsEmpty
        {
            get { return _isEmpty; }
            set { SetProperty(ref _isEmpty, value); }
        }

        public AccountViewModel(Account item = null)
        {
            Title = item?.Name;
            Item = item;

            Transactions = new ObservableCollection<Transaction>();
            LoadTransactionsCommand = new Command(async () => await LoadData(LoadTransactionsAsync));
            DeleteTransactionCommand = new Command((obj) => ConfirmDeletion(obj));

            MessagingCenter.Subscribe<NewTransactionViewModel, ModelAction<Transaction>>(this, Messages.Add, (obj, arg) =>
            {
                Transactions.Insert(0, arg.Item);
                TouchIsEmpty();
            });
        }

        public async Task DeleteAsync(Transaction transaction)
        {
            await Database.DeleteAsync(transaction);
            Transactions.Remove(transaction);
            TouchIsEmpty();
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
            TouchIsEmpty();
        }

        private void ConfirmDeletion(object obj)
        {
            if (obj is Transaction transaction)
            {
                MessagingCenter.Send(this, Messages.Confirmation, transaction);
            }
        }

        private void TouchIsEmpty()
        {
            IsEmpty = !Transactions.Any();
        }
    }
}