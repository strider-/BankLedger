using BankLedger.Models;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BankLedger.ViewModels
{
    public class NewTransactionViewModel : BaseViewModel
    {
        public string Description { get; set; }

        public double Amount { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }

        public Command SaveTransactionCommand { get; set; }

        private int AccountId { get; }

        private bool _isCredit = false;
        public bool IsCredit
        {
            get { return _isCredit; }
            set { SetProperty(ref _isCredit, value); }
        }

        public NewTransactionViewModel(Account account)
        {
            Title = $"{account.Name} Transaction";
            AccountId = account.Id;
            Date = DateTime.Now;
            Time = Date.TimeOfDay;

            SaveTransactionCommand = new Command(async () => await SaveTransactionAsync());
        }

        private async Task SaveTransactionAsync()
        {
            var item = new Transaction
            {
                AccountId = AccountId,
                Amount = Amount * (IsCredit ? 1 : -1),
                Description = Description,
                Timestamp = new DateTime(Date.Year, Date.Month, Date.Day, Time.Hours, Time.Minutes, Time.Seconds)
            };

            await Database.SaveAsync(item);
            MessagingCenter.Send(this, Messages.Add, new ModelAction<Transaction>(item, ActionType.Add));
        }
    }
}