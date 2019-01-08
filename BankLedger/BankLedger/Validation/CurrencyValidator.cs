using System.Globalization;

namespace BankLedger.Validation
{
    public class CurrencyValidator : IValidator
    {
        public string Message { get; set; } = "Invalid currency value";

        public NumberStyles NumberStyle => NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;

        public bool Check(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                // don't display error when our ZeroToEmptyConverter kicks in.
                // the command should still be invalid provided the view model does its job
                return true;
            }

            return double.TryParse(value, NumberStyle, null, out _);
        }
    }
}
