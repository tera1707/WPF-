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
        public DelegateCommand LoadedCommand { get; }

        public UserControl2ViewModel()
        {
            Debug.WriteLine("画面２ コンストラクタ");

            this.LoadedCommand = new DelegateCommand(() =>
            {
                Debug.WriteLine("画面２ LoadedCommand");
            });

            this.ButtonCommand = new DelegateCommand(() =>
            {
                this.RegionManager.RequestNavigate("RedRegion", nameof(UserControl3), new NavigationParameters($"id=1"));
            });
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;
        public void OnNavigatedFrom(NavigationContext navigationContext) => Debug.WriteLine("画面２ NavigatedFrom");
        public void OnNavigatedTo(NavigationContext navigationContext) => Debug.WriteLine("画面２ NavigatedTo");
    }
}
