using System;
using Xamarin.Forms;

namespace BankLedger.Core.Models
{
    public enum MenuItemType
    {
        RecurringTransactions = 1000
    }

    public class HomeMenuItem : NotifyPropertyChanged
    {
        public HomeMenuItem(Type targetPageType)
        {
            TargetPageType = targetPageType ?? throw new ArgumentNullException(nameof(targetPageType));

            if (!typeof(ContentPage).IsAssignableFrom(TargetPageType))
            {
                throw new ArgumentException(nameof(targetPageType), $"Needs to inherit from {nameof(ContentPage)}");
            }
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public Type TargetPageType { get; }

        public virtual ContentPage CreateContentPage() => CreatePageInstance();

        protected ContentPage CreatePageInstance(params object[] args) => (ContentPage)Activator.CreateInstance(TargetPageType, args);
    }

    public class AccountMenuItem : HomeMenuItem
    {
        public AccountMenuItem(Type targetPageType) : base(targetPageType) { }

        private double _balance;
        public double Balance
        {
            get { return _balance; }
            set { SetProperty(ref _balance, value); }
        }

        public Account Account { get; set; }

        public override ContentPage CreateContentPage() => CreatePageInstance(Account);
    }
}