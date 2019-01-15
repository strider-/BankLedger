using BankLedger.Core.Extensions;
using BankLedger.Core.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace BankLedger.Core.Converters
{
    public class TransactionColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Transaction trans && trans.RecurringTransactionId.HasValue)
            {
                return this.RecurringTransactionHighlight();
            }

            return Color.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
