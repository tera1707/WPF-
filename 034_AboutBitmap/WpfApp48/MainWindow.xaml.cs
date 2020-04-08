using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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
            using (var bmp = new Bitmap(@"input.bmp"))
            //using (var fs = new FileStream(@"output1.bmp", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            using (var fs = new MemoryStream())
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    g.DrawString("あいうえお", new Font("Arial", 16), System.Drawing.Brushes.Red, new PointF(10.0f, 10.0f));
                    g.DrawRectangle(Pens.Red, new System.Drawing.Rectangle(100, 100, 100, 100));
                }

                //// 保存方法① streamで保存
                fs.SetLength(0);
                bmp.Save(fs, System.Drawing.Imaging.ImageFormat.Bmp);

                // 保存方法② ファイルに保存
                //bmp.Save(@"output2.bmp");

                // 画面に表示
                var a = BitmapFrame.Create(fs, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                MyImage.Source = a;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // 画面からRenderTargetBitmapに画像を描画
            var canvas = new RenderTargetBitmap((int)MyImage.ActualWidth, (int)MyImage.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            canvas.Render(MyImage);

            using (var stream = new MemoryStream())
            {
                // MemoryStreamに、RenderTargetBitmapから画像を流し込む
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(canvas));
                encoder.Save(stream);

                // MemoryStreamからBitmapを作成
                var editted = new System.Drawing.Bitmap(stream);

                // GraphicsでBitmapを編集(四角を書き込む)
                using (var g = Graphics.FromImage(editted))
                {
                    g.DrawRectangle(Pens.Green, new System.Drawing.Rectangle(3, 3, 200, 200));
                }// →この時点で、streamに緑の四角が書き込まれてる

                // 四角を書き込んだ画像をstreamに流し込む
                editted.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                editted.Dispose();

                // MemoryStreamからBitmapを作成から、BitmapFrame(BitmapSourceの子クラス)を作成
                stream.Seek(0, SeekOrigin.Begin);// seekでBeginに戻さないと例外
                var bitmapSource = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                // それをImageのSourceにセット → 表示！
                MyImage.Source = bitmapSource;
            }
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
