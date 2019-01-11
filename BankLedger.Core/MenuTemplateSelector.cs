using BankLedger.Core.Models;
using Xamarin.Forms;

namespace BankLedger.Core
{
    public class MenuTemplateSelector : DataTemplateSelector
    {
        public DataTemplate AccountTemplate { get; set; }

        public DataTemplate NonAccountTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            switch (item)
            {
                case AccountMenuItem _:
                    return AccountTemplate;
                default:
                    return NonAccountTemplate;
            }
        }
    }
}