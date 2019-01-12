using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using BankLedger.Core.Data;
using BankLedger.Core.Services;
using Java.Lang;
using System.Linq;

namespace BankLedger.Droid.Jobs
{
    [Service(Name = "BankLedger.Android.RecurringTransactionsJob", Permission = "android.permission.BIND_JOB_SERVICE")]
    public class RecurringTransactionsJob : JobService
    {
        public const int JobId = 0x42069;
        public const string ActionKey = "CompletedRecurringTransactions";

        private WorkTask _task;
        private JobParameters _parameters;

        public static JobInfo.Builder Builder(Context context)
        {
            var javaClass = Class.FromType(typeof(RecurringTransactionsJob));
            var componentName = new ComponentName(context, javaClass);

            return new JobInfo.Builder(JobId, componentName);
        }

        public override bool OnStartJob(JobParameters @params)
        {
            _parameters = @params;

            _task = new WorkTask(this);
            _task.Execute();

            return true;
        }

        public override bool OnStopJob(JobParameters @params)
        {
            if (_task != null && !_task.IsCancelled)
            {
                _task.Cancel(true);
            }
            _task = null;

            return true; // reschedule if cancelled
        }

        class WorkTask : AsyncTask
        {
            private readonly RecurringTransactionsJob _jobService;

            private IDatabase Database { get; }

            public WorkTask(RecurringTransactionsJob jobService)
            {
                Database = new LedgerDatabase();
                _jobService = jobService;
            }

            protected override Object DoInBackground(params Object[] @params)
            {
                try
                {
                    var query = new PendingRecurringTransactionsQuery();
                    var pendingTransactions = Database.ExecuteAsync(query).GetAwaiter().GetResult();

                    if (pendingTransactions.Any())
                    {
                        var command = new BatchInsertTransactionsCommand(pendingTransactions);
                        Database.ExecuteAsync(command).GetAwaiter().GetResult();
                    }

                    return true;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to run recurring transactions job: {e.Message}");
                }

                return false;
            }

            protected override void OnPostExecute(Object result)
            {
                base.OnPostExecute(result);
                var success = (bool)result;

                if (success)
                {
                    var intent = new Intent(ActionKey);
                    _jobService.BaseContext.SendBroadcast(intent);
                }

                _jobService.JobFinished(_jobService._parameters, !success);
            }
        }
    }
}