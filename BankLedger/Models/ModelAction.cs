namespace BankLedger.Core.Models
{
    public enum ActionType
    {
        Add,
        Delete,
        None
    };

    public class ModelAction<T>
    {
        public ModelAction(T item, ActionType action)
        {
            Item = item;
            Action = action;
        }

        public T Item { get; }

        public ActionType Action { get; }
    }

    public class EmptyAction : ModelAction<int>
    {
        public EmptyAction() : base(0, ActionType.None) { }
    }
}