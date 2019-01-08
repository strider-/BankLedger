using System;
using System.Globalization;
using Xamarin.Forms;

namespace BankLedger
{
    public class AmountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var amount = (double)value;

            if (amount > 0)
            {
                return Color.Green;
            }
            else if (amount < 0)
            {
                return Color.Red;
            }

            return Color.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
