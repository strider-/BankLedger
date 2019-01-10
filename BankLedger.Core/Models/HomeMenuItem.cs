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

        private string _subtitle;
        public string Subtitle
        {
            get { return _subtitle; }
            set { SetProperty(ref _subtitle, value); }
        }

        public Type TargetType { get; set; }

        public object Data { get; set; }
    }
}