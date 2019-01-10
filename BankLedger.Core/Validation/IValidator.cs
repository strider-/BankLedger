namespace BankLedger.Core.Validation
{
    public interface IValidator
    {
        string Message { get; set; }

        bool Check(string value);
    }
}
