using Android.App;
using Android.App.Job;
using Android.Content.PM;
using Android.OS;
using BankLedger.Core;
using System;

namespace BankLedger.Droid
{
    [Activity(Label = "BankLedger", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            InitJobs((JobScheduler)GetSystemService(JobSchedulerService));
        }

        private void InitJobs(JobScheduler jobScheduler)
        {
            var interval = (long)TimeSpan.FromDays(1).TotalMilliseconds;

            var job = RecurringTransactionsJob.Builder(this)
                .SetPeriodic(interval)
                .Build();

            jobScheduler.Schedule(job);
        }
    }
}