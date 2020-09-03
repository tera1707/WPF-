using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using PrismSample.Views;

namespace PrismSample.ViewModels
{
    class ShellViewModel : BindableBase
    {
        [Dependency]
        public IRegionManager RegionManager { get; set; }

        public DelegateCommand LoadedCommand { get; }
        
        public ShellViewModel()
        {
            this.LoadedCommand = new DelegateCommand(() =>
            {
                this.RegionManager.RequestNavigate("GreenRegion", nameof(UserControl3));
            });
        }

    }
}
