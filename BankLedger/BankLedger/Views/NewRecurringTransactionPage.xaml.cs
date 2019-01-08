using BankLedger.Models;
using BankLedger.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BankLedger.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewRecurringTransactionPage : ContentPage
    {
        private NewRecurringTransactionViewModel _viewModel;

        public NewRecurringTransactionPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new NewRecurringTransactionViewModel();
            MessagingCenter.Subscribe<NewRecurringTransactionViewModel, ModelAction<RecurringTransaction>>(this, Messages.Add, async (obj, trans) =>
            {
                await DismissAsync();
            });
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<NewRecurringTransactionViewModel, ModelAction<RecurringTransaction>>(this, Messages.Add);
        }

        private async void DismissAsync(object sender, EventArgs e) => await DismissAsync();

        private async Task DismissAsync()
        {
            await Navigation.PopModalAsync();
        }
    }
}