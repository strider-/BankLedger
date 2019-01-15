using Xamarin.Forms;

namespace BankLedger.Core.Extensions
{
    public static class ValueConverterExtensions
    {
        private static T GetApplicationResource<T>(this IValueConverter converter, string key)
        {
            return (T)Application.Current.Resources[key];
        }

        public static Color PositiveBalanceColor(this IValueConverter converter) => 
            converter.GetApplicationResource<Color>("PositiveBalanceColor");

        public static Color NegativeBalanceColor(this IValueConverter converter) => 
            converter.GetApplicationResource<Color>("NegativeBalanceColor");

        public static Color ZeroBalanceColor(this IValueConverter converter) =>
            converter.GetApplicationResource<Color>("ZeroBalanceColor");

        public static Color RecurringTransactionHighlight(this IValueConverter converter) =>
            converter.GetApplicationResource<Color>("RecurringTransactionHighlight");
    }
}
