using Android.App;
using Android.App.Job;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using BankLedger.Core;
using BankLedger.Core.Models;
using BankLedger.Droid.Jobs;
using System;
using Xamarin.Forms;

namespace BankLedger.Droid
{
    [Activity(Label = "BankLedger", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        RecurringTransactionsJobReceiver _receiver;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            _receiver = new RecurringTransactionsJobReceiver(this);
            base.OnCreate(savedInstanceState);
            Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            InitJobs((JobScheduler)GetSystemService(JobSchedulerService));
        }

        private void InitJobs(JobScheduler jobScheduler)
        {
            if (jobScheduler.GetPendingJob(RecurringTransactionsJob.JobId) == null)
            {
                var interval = (long)TimeSpan.FromDays(1).TotalMilliseconds;

                var job = RecurringTransactionsJob.Builder(this)
                    .SetPeriodic(interval)
                    .SetPersisted(true)
                    .Build();

                jobScheduler.Schedule(job);
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            var filter = new IntentFilter();
            filter.AddAction(RecurringTransactionsJob.ActionKey);
            RegisterReceiver(_receiver, filter);
        }

        protected override void OnPause()
        {
            UnregisterReceiver(_receiver);
            base.OnPause();
        }

        public void HardRefresh()
        {
            var toast = Toast.MakeText(this, "Daily recurring transactions complete.", ToastLength.Short);
            RunOnUiThread(() => toast.Show());
            MessagingCenter.Send(string.Empty, Messages.HardRefresh, new EmptyAction());
        }
    }
}