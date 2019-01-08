using BankLedger.Extensions;
using BankLedger.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BankLedger.ViewModels
{
    public class NewRecurringTransactionViewModel : BaseViewModel
    {
        private Account _selectedAccount;
        public Account SelectedAccount
        {
            get { return _selectedAccount; }
            set { SetProperty(ref _selectedAccount, value); }
        }

        private string _desc;
        public string Description
        {
            get { return _desc; }
            set { SetProperty(ref _desc, value); }
        }

        private double _amount;
        public double Amount
        {
            get { return _amount; }
            set { SetProperty(ref _amount, value); }
        }

        private DayOfMonth _selectedDay;
        public DayOfMonth SelectedDay
        {
            get { return _selectedDay; }
            set { SetProperty(ref _selectedDay, value); }
        }

        private bool _isCredit = false;
        public bool IsCredit
        {
            get { return _isCredit; }
            set { SetProperty(ref _isCredit, value); }
        }

        public List<DayOfMonth> AvailableDays { get; set; }

        public List<Account> Accounts { get; set; }

        public Command SaveRecurringTransactionCommand { get; set; }

        public NewRecurringTransactionViewModel()
        {
            Title = "New Recurring";
            Accounts = Database.GetAllAsync<Account>().GetAwaiter().GetResult().OrderBy(a => a.Name).ToList();
            AvailableDays = Enumerable.Range(1, 28).Select(i => new DayOfMonth
            {
                Text = i.Place(),
                Value = i
            }).ToList();
            SelectedDay = AvailableDays.First();

            SaveRecurringTransactionCommand = new Command(async () => await SaveTransactionAsync(), canExecute: Valid);
            PropertyChanged += (s, e) => SaveRecurringTransactionCommand.ChangeCanExecute();
        }

        private bool Valid()
        {
            return SelectedAccount != null &&
                   !string.IsNullOrWhiteSpace(Description) &&
                   Amount > 0 &&
                   SelectedDay != null;
        }

        private async Task SaveTransactionAsync()
        {
            var item = new RecurringTransaction
            {
                AccountId = SelectedAccount.Id,
                Amount = Amount * (IsCredit ? 1 : -1),
                Description = Description,
                Day = SelectedDay.Value
            };

            await Database.SaveAsync(item);
            MessagingCenter.Send(this, Messages.Add, new ModelAction<RecurringTransaction>(item, ActionType.Add));
        }

        public class DayOfMonth
        {
            public string Text { get; set; }

            public int Value { get; set; }
        }
    }
}
