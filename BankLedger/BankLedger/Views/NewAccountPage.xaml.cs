using BankLedger.Models;
using BankLedger.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BankLedger.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewAccountPage : ContentPage
    {
        private MainPage RootPage { get => Application.Current.MainPage as MainPage; }

        private NewAccountViewModel _viewModel;

        public NewAccountPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new NewAccountViewModel();

            MessagingCenter.Subscribe<NewAccountViewModel, Account>(this, Messages.Add, async (obj, act) => await GoToNewAccountAsync(obj, act));
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<NewAccountViewModel, Account>(this, Messages.Add);
        }

        private async Task GoToNewAccountAsync(NewAccountViewModel vm, Account account)
        {
            await DismissAsync();
            await RootPage.NavigateToAsync(new AccountPage(account));
        }

        private async void DismissAsync(object sender, EventArgs e) => await DismissAsync();

        private async Task DismissAsync()
        {
            await Navigation.PopModalAsync();
        }
    }
}