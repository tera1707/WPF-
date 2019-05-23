using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace PrismSample.ViewModels
{
    class UserControl1ViewModel : BindableBase, INavigationAware
    {
        // ボタン押下時処理
        public DelegateCommand ButtonCommand { get; }

        // ★〇の色を切り替えるためのフラグ
        private bool _colorChangeFlag = false;
        public bool ColorChangeFlag
        {
            get { return _colorChangeFlag; }
            set { SetProperty(ref _colorChangeFlag, value); }
        }

        // コンストラクタ
        public UserControl1ViewModel()
        {
            this.ButtonCommand = new DelegateCommand(() =>
            {
                // ★ボタンをおしたら、フラグが切り替わる
                ColorChangeFlag = !ColorChangeFlag;
            });
        }

        // --------- Prismお決まり部分 -------------

        [Dependency]
        public IRegionManager RegionManager { get; set; }

        public bool IsNavigationTarget(NavigationContext navigationContext) => false;
        public void OnNavigatedFrom(NavigationContext navigationContext) { }
        public void OnNavigatedTo(NavigationContext navigationContext) { }
    }
}
