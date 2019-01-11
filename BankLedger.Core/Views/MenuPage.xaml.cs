using BankLedger.Core.Models;
using BankLedger.Core.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BankLedger.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        private MainPage RootPage { get => Application.Current.MainPage as MainPage; }

        private MenuViewModel _viewModel;

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
            if (e.SelectedItem is HomeMenuItem item && item.TargetPageType != null)
            {
                var page = CreatePageFromMenuItem(item);

                await RootPage.NavigateToAsync(page);
                ListViewMenu.SelectedItem = null;
            }
        }

        private ContentPage CreatePageFromMenuItem(HomeMenuItem item)
        {
            List<object> args = new List<object>();

            if (item is AccountMenuItem acm)
            {
                args.Add(acm.Account);
            }

            return (ContentPage)Activator.CreateInstance(item.TargetPageType, args.ToArray());
        }

        private async void OnAddAccountAsync(object sender, EventArgs e)
        {
            await RootPage.ShowAddAccountModalAsync();
        }
    }
}