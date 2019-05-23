using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using PrismSample.Views;
using System.Diagnostics;

namespace PrismSample.ViewModels
{
    class UserControl2ViewModel : BindableBase, INavigationAware
    {
        [Dependency]
        public IRegionManager RegionManager { get; set; }

        public DelegateCommand ButtonCommand { get; }

        public UserControl2ViewModel()
        {
            this.ButtonCommand = new DelegateCommand(() =>
            {
                this.RegionManager.RequestNavigate("MainRegion", nameof(UserControl1), new NavigationParameters($"id=1"));
            });
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            Debug.WriteLine("画面２ NavigatedFrom");
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Debug.WriteLine("画面２ NavigatedTo");
            string Id = navigationContext.Parameters["id"] as string;
        }
    }
}
