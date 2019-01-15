using BankLedger.Core.Extensions;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace BankLedger.Core.Converters
{
    public class AmountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var amount = (double)value;

            if (amount > 0)
            {
                return this.PositiveBalanceColor();
            }
            else if (amount < 0)
            {
                return this.NegativeBalanceColor();
            }

            return this.ZeroBalanceColor();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}