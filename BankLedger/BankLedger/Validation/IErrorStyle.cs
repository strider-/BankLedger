using Xamarin.Forms;

namespace BankLedger.Validation
{
    public interface IErrorStyle
    {
        void ShowError(View view, string message);

        void RemoveError(View view);
    }
}
