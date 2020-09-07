using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using PrismSample.Views;
using System.Diagnostics;

namespace PrismSample.ViewModels
{
    class UserControl1ViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        [Dependency]
        public IRegionManager RegionManager { get; set; }
        public DelegateCommand ButtonCommand { get; }
        public DelegateCommand ButtonKeepAliveONCommand { get; }
        public DelegateCommand ButtonKeepAliveOFFCommand { get; }
        public DelegateCommand ButtonIsNavigationTargetONCommand { get; }
        public DelegateCommand ButtonIsNavigationTargetOFFCommand { get; }
        public DelegateCommand LoadedCommand { get; }

        private static int constructorCounter = 0;
        private static int destructorCounter = 0;

        public bool KeepAlive
        {
            get
            {
                Debug.WriteLine("画面１ KeepAlive is " + keepalive);
                return keepalive;
            }
            set
            {
                keepalive = value;
            }
        }
        private bool keepalive = true;
        public bool IsNavigationTargetFlag = true;

        public UserControl1ViewModel()
        {
            constructorCounter++;
            Debug.WriteLine("画面１ コンストラクタ " + constructorCounter + " 個目");

            this.LoadedCommand = new DelegateCommand(() =>
            {
                Debug.WriteLine("画面１ LoadedCommand");
            });

            this.ButtonCommand = new DelegateCommand(() =>
            {
                // Shell.xaml.csで作成したリージョンの名前と、画面のUserControlクラス名を指定して、画面遷移させる。
                // (パラメータを渡すこともできる)
                this.RegionManager.RequestNavigate("RedRegion", nameof(UserControl2), new NavigationParameters($"id=1"));
            });

            this.ButtonKeepAliveONCommand = new DelegateCommand(() => KeepAlive = true);
            this.ButtonKeepAliveOFFCommand = new DelegateCommand(() => KeepAlive = false);

            this.ButtonIsNavigationTargetONCommand = new DelegateCommand(() => IsNavigationTargetFlag = true);
            this.ButtonIsNavigationTargetOFFCommand = new DelegateCommand(() => IsNavigationTargetFlag = false);
        }

        ~UserControl1ViewModel()
        {
            destructorCounter++;
            Debug.WriteLine("画面１ デストラクタ " + destructorCounter + " 回目");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            Debug.WriteLine("画面１ IsNavigationTarget  return value is" + IsNavigationTargetFlag);
            // このメソッドの返す値により、画面のインスタンスを使いまわすかどうか制御できる。
            // true ：インスタンスを使いまわす(画面遷移してもコンストラクタ呼ばれない)
            // false：インスタンスを使いまわさない(画面遷移するとコンストラクタ呼ばれる)
            // メソッド実装なし：trueになる
            return IsNavigationTargetFlag;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // この画面から他の画面に遷移するときの処理
            Debug.WriteLine("画面１ NavigatedFrom");
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            // 他の画面からこの画面に遷移したときの処理
            Debug.WriteLine("画面１ NavigatedTo");

            // 画面遷移元から、この画面に遷移したときにパラメータを受け取れる。
            string Id = navigationContext.Parameters["id"] as string;
        }
    }
}
