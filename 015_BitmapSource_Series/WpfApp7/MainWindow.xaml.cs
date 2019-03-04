using System;
using System.Collections.Generic;
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
using Microsoft.Win32;

namespace WpfApp7
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Title = "BitmapSource派生クラスの使い方";
        }

        /// <summary>
        /// RenderTargetBitmapの使い方
        /// UI部品(FlameworkElement)を画像として取り込むときに使う
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.Filter = "Jpeg ファイル(.jpg)|*.jpg|All Files (*.*)|*.*";
            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                var canvas = new RenderTargetBitmap((int)MyCanvas.Width, (int)MyCanvas.Height, 96, 96, PixelFormats.Pbgra32);
                canvas.Render(MyCanvas);

                var jpg = new JpegBitmapEncoder();
                jpg.Frames.Add(BitmapFrame.Create(canvas));
                using (Stream stm = File.Create(saveFileDialog.FileName))
                {
                    jpg.Save(stm);
                }
            }
        }

        /// <summary>
        /// BitmapImageの使い方
        /// ファイルから画像を引っ張ってくるときに使う
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.FilterIndex = 1;
            openFileDialog.Filter = "Jpeg ファイル(.jpg)|*.jpg|All Files (*.*)|*.*";
            bool? result = openFileDialog.ShowDialog();

            if (result != false)
            {
                tb_FileName.Text = openFileDialog.FileName;
                var uri = new Uri(tb_FileName.Text, UriKind.Absolute);
                var image = new BitmapImage(uri);
                MyImage.Source = image;
            }
        }

        /// <summary>
        /// TransformedBitmapの使い方
        /// 各種Transform派生クラスと組み合わせて、回転、拡大等するときに使う
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.Filter = "Jpeg ファイル(.jpg)|*.jpg|All Files (*.*)|*.*";
            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                // 画像を取ってきて
                var canvas = new RenderTargetBitmap((int)MyCanvas.Width, (int)MyCanvas.Height, 96, 96, PixelFormats.Pbgra32);
                canvas.Render(MyCanvas);

                // 1/2にタテヨコ縮小して
                var sct = new ScaleTransform(0.5, 0.5);
                var canvas2 = new TransformedBitmap(canvas, sct);

                // 90度回転して
                var rtt = new RotateTransform(90);
                var canvas3 = new TransformedBitmap(canvas2, rtt);

                // 保存する
                var jpg = new JpegBitmapEncoder();
                jpg.Frames.Add(BitmapFrame.Create(canvas3));
                using (Stream stm = File.Create(saveFileDialog.FileName))
                {
                    jpg.Save(stm);
                }
            }
        }
    }
}
