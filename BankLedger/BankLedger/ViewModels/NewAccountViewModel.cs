using BankLedger.Models;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BankLedger.ViewModels
{
    public class NewAccountViewModel : BaseViewModel
    {
        public string Name { get; set; }

        public double InitialBalance { get; set; }

        public Command SaveAccountCommand { get; set; }

        public NewAccountViewModel()
        {
            Title = "Create Account";

            SaveAccountCommand = new Command(async () => await SaveAccountAsync());
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