using BankLedger.Models;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BankLedger.ViewModels
{
    public class NewTransactionViewModel : BaseViewModel
    {
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

            SaveTransactionCommand = new Command(async () => await SaveTransactionAsync(), canExecute: Valid);
            PropertyChanged += (s, e) => SaveTransactionCommand.ChangeCanExecute();
        }

        private bool Valid()
        {
            return !string.IsNullOrWhiteSpace(Description) && 
                Amount > 0 && 
                AccountId != 0;
        }

        private async Task SaveTransactionAsync()
        {
            var item = new Transaction
            {
                AccountId = AccountId,
                Amount = Amount * (IsCredit ? 1 : -1),
                Description = Description,
                Timestamp = new DateTime(Date.Year, Date.Month, Date.Day, Time.Hours, Time.Minutes, Time.Seconds, DateTimeKind.Local)
            };

            await Database.SaveAsync(item);
            MessagingCenter.Send(this, Messages.Add, new ModelAction<Transaction>(item, ActionType.Add));
        }
    }
}