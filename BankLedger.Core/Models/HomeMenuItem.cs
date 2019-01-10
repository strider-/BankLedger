using System;

namespace BankLedger.Core.Models
{
    public enum MenuItemType
    {
        RecurringTransactions = 1000
    }

    public class HomeMenuItem : NotifyPropertyChanged
    {
        public int Id { get; set; }

        public string Title { get; set; }

        private double _balance;
        public double Balance
        {
            get { return _balance; }
            set { SetProperty(ref _balance, value); }
        }

        public Type TargetPageType { get; set; }

        public bool IsForAccount => Data != null && Data is Account;

        public object Data { get; set; }
    }
}