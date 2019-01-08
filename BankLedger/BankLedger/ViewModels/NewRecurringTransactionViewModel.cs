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
        public Account SelectedAccount { get; set; }

        public string Description { get; set; }

        public double Amount { get; set; }

        public DayOfMonth SelectedDay { get; set; }

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

            SaveRecurringTransactionCommand = new Command(async () => await SaveTransactionAsync());
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
