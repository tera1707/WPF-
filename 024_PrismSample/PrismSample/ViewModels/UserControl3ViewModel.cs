using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using PrismSample.Views;
using System;
using System.Diagnostics;

namespace PrismSample.ViewModels
{
    class UserControl3ViewModel : BindableBase, INavigationAware
    {
        [Dependency]
        public IRegionManager RegionManager { get; set; }
        public DelegateCommand<object> ButtonCommand { get; }
        public DelegateCommand LoadedCommand { get; }

        public UserControl3ViewModel()
        {
            Debug.WriteLine("画面３ コンストラクタ");

            this.LoadedCommand = new DelegateCommand(() =>
            {
                Debug.WriteLine("画面３ LoadedCommand");
            });

            this.ButtonCommand = new DelegateCommand<object>((param) =>
            {
                var kind = int.Parse((string)param);
                switch (kind)
                {
                    default:
                    case 0: RegionManager.RequestNavigate("RedRegion", nameof(UserControl1), new NavigationParameters($"id=1")); break;
                    case 1: RegionManager.RequestNavigate("RedRegion", nameof(UserControl2), new NavigationParameters($"id=1")); break;
                    case 2: RegionManager.RequestNavigate("RedRegion", nameof(UserControl3), new NavigationParameters($"id=1")); break;
                    case 3: RegionManager.RequestNavigate("BlueRegion", nameof(UserControl1), new NavigationParameters($"id=1")); break;
                    case 4: RegionManager.RequestNavigate("BlueRegion", nameof(UserControl2), new NavigationParameters($"id=1")); break;
                    case 5: RegionManager.RequestNavigate("BlueRegion", nameof(UserControl3), new NavigationParameters($"id=1")); break;
                    case 10: RegionManager.Regions["RedRegion"].RemoveAll(); break;
                    case 11: RegionManager.Regions["BlueRegion"].RemoveAll(); break;
                    case 91: GC.Collect(); break;
                }

            });
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => false;
        public void OnNavigatedFrom(NavigationContext navigationContext) => Debug.WriteLine("画面３ NavigatedFrom");
        public void OnNavigatedTo(NavigationContext navigationContext) => Debug.WriteLine("画面３ NavigatedTo");
    }
}
