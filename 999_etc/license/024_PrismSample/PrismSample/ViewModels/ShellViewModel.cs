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

        public DelegateCommand ButtonCommand { get; }
        
        public ShellViewModel()
        {
            this.ButtonCommand = new DelegateCommand(() =>
            {
                this.RegionManager.RequestNavigate("MainRegion", nameof(UserControl1));
            });
        }

    }
}
