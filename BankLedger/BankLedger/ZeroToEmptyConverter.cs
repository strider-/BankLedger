using System;
using System.Globalization;
using Xamarin.Forms;

namespace BankLedger
{
    public class ZeroToEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double result && result > 0)
            {
                return result;
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strValue = value as string;

            if (double.TryParse(strValue, out var realValue))
            {
                return realValue;
            }

            return 0;
        }
    }
}