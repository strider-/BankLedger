using BankLedger.Core.Models;
using BankLedger.Core.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BankLedger.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecurringTransactionsPage : ContentPage
    {
        private RecurringTransactionsViewModel _viewModel;

        public RecurringTransactionsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new RecurringTransactionsViewModel();

            MessagingCenter.Subscribe<RecurringTransactionsViewModel, RecurringTransaction>(this, Messages.Confirmation, ConfirmDeleteAsync);
        }

        public async void OnAddRecurringTransactionAsync(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewRecurringTransactionPage()));
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_viewModel.Summaries.Count == 0)
            {
                _viewModel.LoadSummariesCommand.Execute(null);
            }
        }

        public async void ConfirmDeleteAsync(RecurringTransactionsViewModel viewModel, RecurringTransaction transaction)
        {
            var result = await DisplayAlert("Delete Recurring", $"Delete {transaction.Description} in the amount of {transaction.Amount:C}?", "Delete", "Cancel");
            if (result)
            {
                await viewModel.DeleteAsync(transaction);
            }
        }
    }
}