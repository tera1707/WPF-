using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using PrismSample.Views;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
                //this.RegionManager.RequestNavigate("MainRegion", nameof(UserControl2), new NavigationParameters($"id=1"));

                Debug.WriteLine($"{MyProperty}");
                CaretIndex = 1;
            });
        }

        public int MyProperty { get; set; }

        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                int ca = CaretIndex;
                string k = value;

                var wholeKey = k.Replace("-", "");

                int len = wholeKey.Length;
                if (len > 24) wholeKey = wholeKey.Insert(24, "-");
                if (len > 16) wholeKey = wholeKey.Insert(16, "-");
                if (len > 8) wholeKey = wholeKey.Insert(8, "-");


                SetProperty(ref _key, wholeKey);

                CaretIndex = ca;
            }
        }
        private string _key = "12345678-90123456-78901234-56789012";

        public int CaretIndex {
            get
            {
                return _caretIndex;
            }
            set
            {
                SetProperty(ref _caretIndex, value);
            }
        }
        private int _caretIndex = 0;

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            // このメソッドの返す値により、画面のインスタンスを使いまわすかどうか制御できる。
            // true ：インスタンスを使いまわす(画面遷移してもコンストラクタ呼ばれない)
            // false：インスタンスを使いまわさない(画面遷移するとコンストラクタ呼ばれる)
            // メソッド実装なし：trueになる
            return true;
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

    public class MyTextBox : TextBox
    {
        public static readonly DependencyProperty CaretPositionProperty =
            DependencyProperty.Register("CaretPosition", typeof(int), typeof(MyTextBox),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCaretPositionChanged));

        public int CaretPosition
        {
            get { return (int)GetValue(CaretPositionProperty); }
            set { SetValue(CaretPositionProperty, value); }
        }

        public MyTextBox()
        {
            SelectionChanged += (s, e) => CaretPosition = CaretIndex;
        }

        private static void OnCaretPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MyTextBox).CaretIndex = (int)e.NewValue;
        }
    }
}
