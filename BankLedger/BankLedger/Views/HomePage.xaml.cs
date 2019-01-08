using BankLedger.Models;
using BankLedger.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BankLedger.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private MainPage RootPage { get => Application.Current.MainPage as MainPage; }

        private HomePageViewModel _viewModel;

        public HomePage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new HomePageViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_viewModel.Items.Count == 0)
            {
                _viewModel.LoadAccountsCommand.Execute(null);
            }
        }

        private async void OnAccountSelectedAsync(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Account account)
            {
                var page = new AccountPage(account);

                await RootPage.NavigateToAsync(page);
            }
        }

        private async void OnAddAccountAsync(object sender, EventArgs e)
        {
            await RootPage.ShowAddAccountModalAsync();
        }
    }
}