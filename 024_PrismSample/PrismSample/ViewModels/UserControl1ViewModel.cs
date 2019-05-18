using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using PrismSample.Views;

namespace PrismSample.ViewModels
{
    class UserControl1ViewModel : BindableBase, INavigationAware
    {
        [Dependency]
        public IRegionManager RegionManager { get; set; }

        public DelegateCommand ButtonCommand { get; }

        public UserControl1ViewModel()
        {
            this.ButtonCommand = new DelegateCommand(() =>
            {
                // Shell.xaml.csで作成したリージョンの名前と、画面のUserControlクラス名を指定して、画面遷移させる。
                // (パラメータを渡すこともできる)
                this.RegionManager.RequestNavigate("MainRegion", nameof(UserControl2), new NavigationParameters($"id=1"));
            });
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            // このメソッドの返す値により、画面のインスタンスを使いまわすかどうか制御できる。
            // true ：インスタンスを使いまわす(画面遷移してもコンストラクタ呼ばれない)
            // false：インスタンスを使いまわさない(画面遷移するとコンストラクタ呼ばれる)
            // メソッド実装なし：trueになる
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // この画面から他の画面に遷移するときの処理
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            // 他の画面からこの画面に遷移したときの処理

            // 画面遷移元から、この画面に遷移したときにパラメータを受け取れる。
            string Id = navigationContext.Parameters["id"] as string;
        }
    }
}
