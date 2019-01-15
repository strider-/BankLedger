using BankLedger.Core.Services;
using BankLedger.Core.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BankLedger.Core
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
    }
}
