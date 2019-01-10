using BankLedger.Core.Models;
using BankLedger.Core.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BankLedger.Core.ViewModels
{
    public class BaseViewModel : NotifyPropertyChanged
    {
        public IDatabase Database => App.Database;

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected async Task LoadData(Func<Task> task)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                await Task.Delay(100);
                await task();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
