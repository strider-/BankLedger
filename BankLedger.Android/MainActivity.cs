using Android.App;
using Android.App.Job;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using System;
using BankLedger.Core;

namespace BankLedger.Droid
{
    [Activity(Label = "BankLedger", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private JobScheduler _jobScheduler;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            
            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            _jobScheduler = (JobScheduler)GetSystemService(JobSchedulerService);
            InitJobs();
        }

        private void InitJobs()
        {
            var javaClass = Java.Lang.Class.FromType(typeof(RecurringTransactionsJob));
            var componentName = new ComponentName(this, javaClass);

            var interval = (long)TimeSpan.FromDays(1).TotalMilliseconds;

            JobInfo.Builder builder = new JobInfo.Builder(0x42069, componentName)
                .SetPeriodic(interval); // once a day

            _jobScheduler.Schedule(builder.Build());
        }
    }
}