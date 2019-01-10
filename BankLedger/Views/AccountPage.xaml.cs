using BankLedger.Core.Models;
using BankLedger.Core.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BankLedger.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountPage : ContentPage
    {
        AccountViewModel _viewModel;

        public AccountPage(Account account)
        {
            InitializeComponent();

            BindingContext = _viewModel = new AccountViewModel(account);

            MessagingCenter.Subscribe<AccountViewModel, Transaction>(this, Messages.Confirmation, ConfirmDeleteAsync);
        }

        public async void OnAddTransactionAsync(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewTransactionPage(_viewModel.Item)));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_viewModel.Transactions.Count == 0)
            {
                _viewModel.LoadTransactionsCommand.Execute(null);
            }
        }

        public async void ConfirmDeleteAsync(AccountViewModel viewModel, Transaction transaction)
        {
            var result = await DisplayAlert("Delete Transaction", $"Delete {transaction.Description} in the amount of {transaction.Amount:C}?", "Delete", "Cancel");
            if (result)
            {
                await viewModel.DeleteAsync(transaction);
            }
        }
    }
}