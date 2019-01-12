using BankLedger.Core.Data;
using BankLedger.Core.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BankLedger.Core.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        public ObservableCollection<Account> Items { get; set; } = new ObservableCollection<Account>();

        public Command LoadAccountsCommand { get; set; }

        private bool _isEmpty;
        public bool IsEmpty
        {
            get { return _isEmpty; }
            set { SetProperty(ref _isEmpty, value); }
        }

        private IDatabaseQuery<IEnumerable<Account>> Query { get; } = new AccountWithCurrentBalanceQuery();


        public HomePageViewModel()
        {
            Title = "Bank Ledger";
            LoadAccountsCommand = new Command(async () => await LoadData(LoadAccountsAsync));
            Items.CollectionChanged += (s, e) => IsEmpty = !Items.Any();

            MessagingCenter.Subscribe<string, EmptyAction>(this, Messages.HardRefresh, (s, e) => LoadAccountsCommand.Execute(null));
        }

        private async Task LoadAccountsAsync()
        {
            Items.Clear();
            var accounts = await Database.ExecuteAsync(Query);
            foreach (var account in accounts)
            {
                Items.Add(account);
            }
        }
    }
}