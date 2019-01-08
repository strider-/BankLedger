namespace BankLedger.Validation
{
    public class RequiredValidator : IValidator
    {
        public string Message { get; set; } = "This field is required";

        public bool Check(string value) => !string.IsNullOrWhiteSpace(value);
    }
}
