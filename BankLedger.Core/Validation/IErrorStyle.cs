using Xamarin.Forms;

namespace BankLedger.Core.Validation
{
    public interface IErrorStyle
    {
        void ShowError(View view, string message);

        void RemoveError(View view);
    }
}
