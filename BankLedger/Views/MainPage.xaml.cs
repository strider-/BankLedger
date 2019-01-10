using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BankLedger.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;
        }

        public async Task NavigateToAsync(ContentPage newPage)
        {
            if (newPage != null && Detail != newPage)
            {
                Detail = new NavigationPage(newPage);

                if (Device.RuntimePlatform == Device.Android)
                {
                    await Task.Delay(100);
                }

                IsPresented = false;
            }
        }

        public async Task ShowAddAccountModalAsync()
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewAccountPage()));
            IsPresented = false;
        }
    }
}