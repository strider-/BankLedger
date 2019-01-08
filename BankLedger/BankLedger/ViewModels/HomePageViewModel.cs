using BankLedger.Data;
using BankLedger.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BankLedger.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        public ObservableCollection<Account> Items { get; set; } = new ObservableCollection<Account>();

        public Command LoadAccountsCommand { get; set; }

        private IDatabaseQuery<IEnumerable<Account>> Query { get; } = new AccountWithCurrentBalanceQuery();

        public HomePageViewModel()
        {
            Title = "Home";
            LoadAccountsCommand = new Command(async () => await LoadData(LoadAccountsAsync));
        }

        private async Task LoadAccountsAsync()
        {
            Items.Clear();
            var accounts = await Database.ExecuteAsync(Query);
            foreach(var account in accounts)
            {
                Items.Add(account);
            }
        }
    }
}