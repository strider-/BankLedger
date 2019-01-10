using BankLedger.Core.Models;
using BankLedger.Core.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BankLedger.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTransactionPage : ContentPage
    {
        private NewTransactionViewModel _viewModel;

        public NewTransactionPage(Account account)
        {
            InitializeComponent();
            BindingContext = _viewModel = new NewTransactionViewModel(account);

            MessagingCenter.Subscribe<NewTransactionViewModel, ModelAction<Transaction>>(this, Messages.Add, async (obj, trans) =>
            {
                await DismissAsync();
            });
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<NewTransactionViewModel, ModelAction<Transaction>>(this, Messages.Add);
        }

        private async void DismissAsync(object sender, EventArgs e) => await DismissAsync();

        private async Task DismissAsync()
        {
            await Navigation.PopModalAsync();
        }
    }
}