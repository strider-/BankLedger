using Android.Content;

namespace BankLedger.Droid.Jobs
{
    [BroadcastReceiver(Enabled = true, Exported = false)]
    public class RecurringTransactionsJobReceiver : BroadcastReceiver
    {
        private MainActivity Activity { get; }

        public RecurringTransactionsJobReceiver() : this(null) { }

        public RecurringTransactionsJobReceiver(MainActivity activity) => Activity = activity;

        public override void OnReceive(Context context, Intent intent)
        {
            Activity?.HardRefresh();
        }
    }
}