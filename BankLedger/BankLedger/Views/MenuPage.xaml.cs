using BankLedger.Models;
using BankLedger.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BankLedger.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }

        MenuViewModel _viewModel;

        public MenuPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new MenuViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_viewModel.Items.Count == 0)
            {
                _viewModel.LoadItemsCommand.Execute(null);
            }
        }

        private async void OnItemSelectedAsync(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is HomeMenuItem item && item.TargetType != null)
            {
                ContentPage page;
                if (item.TargetType == typeof(AccountPage))
                {
                    page = (ContentPage)Activator.CreateInstance(item.TargetType, new[] { (Account)item.Data });
                }
                else
                {
                    page = (ContentPage)Activator.CreateInstance(item.TargetType);
                }

                await RootPage.NavigateToAsync(page);
                ListViewMenu.SelectedItem = null;
            }
        }

        private async void OnAddAccountAsync(object sender, EventArgs e)
        {
            await RootPage.ShowAddAccountModalAsync();
        }
    }
}