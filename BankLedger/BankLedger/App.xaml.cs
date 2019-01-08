using BankLedger.Services;
using BankLedger.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BankLedger
{
    public partial class App : Application
    {
        private static IDatabase _database;

        public App()
        {
            InitializeComponent();

            DependencyService.Register<IDatabase, LedgerDatabase>();

            MainPage = new MainPage();
        }

        public static IDatabase Database
        {
            get { return _database ?? (_database = DependencyService.Get<IDatabase>()); }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
