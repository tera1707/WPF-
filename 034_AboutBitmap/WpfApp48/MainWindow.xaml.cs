using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp48
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


        #region LogFramework
        public ObservableCollection<string> Logs { get; set; } = new ObservableCollection<string>();

        public void AddLog(string log)
        {
            DateTime now = DateTime.Now;

            Logs.Add(now.ToString("hh:mm:ss.fff ") + log);
            OnPropertyChanged(nameof(Logs));
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AddLog("ログ１");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var bmp = new Bitmap(@"img.bmp"))
            using (var fs = new FileStream(@"imgout1.bmp", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    g.DrawString("あいうえお", new Font("Arial", 16), System.Drawing.Brushes.Red, new PointF(10.0f, 10.0f));
                    g.DrawRectangle(Pens.Red, new System.Drawing.Rectangle(100, 100, 100, 100));
                }

                // 保存方法① streamで保存
                fs.SetLength(0);
                bmp.Save(fs, System.Drawing.Imaging.ImageFormat.Bmp);

                // 保存方法② ファイルに保存
                bmp.Save(@"imgout2.bmp");

                // 画面に表示
                var a = BitmapFrame.Create(fs, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                MyImage.Source = a;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AddLog("ログ３");
        }
    }
}
/*
    g.FrawString等の書き方
    https://docs.microsoft.com/ja-jp/dotnet/api/system.drawing.graphics.drawstring?view=netframework-4.8
 
    Bitmapをxamlで画面に表示するまで(ストリームからBitmapFrameに変換)
    https://water2litter.net/gin/?p=979
     
    ビットマップ形式の画像データを相互変換
    https://qiita.com/YSRKEN/items/a24bf2173f0129a5825c
*/
