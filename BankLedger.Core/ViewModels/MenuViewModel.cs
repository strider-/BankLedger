using BankLedger.Core.Data;
using BankLedger.Core.Extensions;
using BankLedger.Core.Models;
using BankLedger.Core.Services;
using BankLedger.Core.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BankLedger.Core.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        public ObservableCollection<HomeMenuItem> Items { get; set; }

        public Command LoadItemsCommand { get; set; }

        private IDatabaseQuery<IEnumerable<Account>> Query { get; }

        private bool _isEmpty;
        public bool IsEmpty
        {
            get { return _isEmpty; }
            set { SetProperty(ref _isEmpty, value); }
        }

        public MenuViewModel()
        {
            Title = "Menu";
            Items = new ObservableCollection<HomeMenuItem>();
            Items.CollectionChanged += (s, e) => IsEmpty = !Items.Any();
            LoadItemsCommand = new Command(async () => await LoadData(LoadMenuItemsAsync));
            Query = new AccountWithCurrentBalanceQuery();

            MessagingCenter.Subscribe<NewAccountViewModel, Account>(this, Messages.Add, AddNewMenuItem);
            MessagingCenter.Subscribe<NewTransactionViewModel, ModelAction<Transaction>>(this, Messages.Add, Refresh);
            MessagingCenter.Subscribe<AccountViewModel, ModelAction<Transaction>>(this, Messages.Delete, Refresh);
            MessagingCenter.Subscribe<IDatabase, EmptyAction>(this, Messages.HardRefresh, HardRefresh);
        }

        private void AddNewMenuItem(object sender, Account account)
        {
            if (ContainsRecurringTransactionsItem())
            {
                Items.Insert(Items.Count - 1, account.ToHomeMenuItem());
            }
            else
            {
                Items.Add(account.ToHomeMenuItem());
            }

            DetermineRecurringTransactionsItem();
        }

        private void HardRefresh(object sender, EmptyAction arg)
        {
            LoadItemsCommand.Execute(null);
        }

        private void Refresh(object sender, ModelAction<Transaction> obj)
        {
            var menuItem = Items.FirstOrDefault(i => i.Id == obj.Item.AccountId);
            if (menuItem != null)
            {
                var index = Items.IndexOf(menuItem);
                var account = menuItem.Data as Account;
                switch (obj.Action)
                {
                    case ActionType.Add:
                        account.CurrentBalance += obj.Item.Amount;
                        break;
                    case ActionType.Delete:
                        account.CurrentBalance -= obj.Item.Amount;
                        break;
                }
                Items.RemoveAt(index);
                Items.Insert(index, menuItem);
            }
        }

        private async Task LoadMenuItemsAsync()
        {
            Items.Clear();
            var accounts = await Database.ExecuteAsync(Query);
            foreach (var account in accounts)
            {
                Items.Add(account.ToHomeMenuItem());
            }

            DetermineRecurringTransactionsItem();
        }

        private void DetermineRecurringTransactionsItem()
        {
            if (AtLeastOneAccount() && !ContainsRecurringTransactionsItem())
            {
                Items.Add(new HomeMenuItem
                {
                    Id = (int)MenuItemType.RecurringTransactions,
                    Title = "Recurring Transactions",
                    TargetType = typeof(RecurringTransactionsPage)
                });
            }
        }

        private bool AtLeastOneAccount() => Items.Any(IsAccountItem);

        private bool IsAccountItem(HomeMenuItem item) => item.TargetType == typeof(AccountPage);

        private bool ContainsRecurringTransactionsItem() => Items.Any(i => i.Id == (int)MenuItemType.RecurringTransactions);
    }
}