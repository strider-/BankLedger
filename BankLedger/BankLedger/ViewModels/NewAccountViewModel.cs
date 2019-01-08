using BankLedger.Models;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BankLedger.ViewModels
{
    public class NewAccountViewModel : BaseViewModel
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private double _initialBalance;
        public double InitialBalance
        {
            get { return _initialBalance; }
            set { SetProperty(ref _initialBalance, value); }
        }

        public Command SaveAccountCommand { get; set; }

        public NewAccountViewModel()
        {
            Title = "Create Account";

            SaveAccountCommand = new Command(async () => await SaveAccountAsync(), canExecute: Valid);
            PropertyChanged += (s, e) => SaveAccountCommand.ChangeCanExecute();
        }

        private bool Valid()
        {
            return !string.IsNullOrWhiteSpace(Name) && InitialBalance > 0;
        }

        public async Task SaveAccountAsync()
        {
            var account = new Account
            {
                CurrentBalance = InitialBalance,
                InitialBalance = InitialBalance,
                Name = Name
            };
            await Database.SaveAsync(account);

            MessagingCenter.Send(this, Messages.Add, account);
        }
    }
}