using System;
using System.Collections.Generic;
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

namespace WpfApp3
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlgSave = new Microsoft.Win32.SaveFileDialog();

            dlgSave.Filter = "ビットマップファイル(*.bmp)|*.bmp|" +
            "JPEGファイル(*.jpg)|*,jpg|" +
            "PNGファイル(*.png)|*.png";
            dlgSave.AddExtension = true;

            if ((bool)dlgSave.ShowDialog())
            {
                // 拡張子を取得する
                string extension = System.IO.Path.GetExtension(dlgSave.FileName).ToUpper();

                // ストロークが描画されている境界を取得
                Rect rectBounds = inkCanvas1.Strokes.GetBounds();

                // 描画先を作成
                DrawingVisual dv = new DrawingVisual();
                DrawingContext dc = dv.RenderOpen();
                // 描画エリアの位置補正（補正しないと黒い部分ができてしまう）
                dc.PushTransform(new TranslateTransform(-rectBounds.X, -rectBounds.Y));

                // 描画エリア(dc)に四角形を作成
                // 四角形の大きさはストロークが描画されている枠サイズとし、
                // 背景色はInkCanvasコントロールと同じにする
                dc.DrawRectangle(inkCanvas1.Background, null, rectBounds);



                var ima = new BitmapImage(new Uri(@"C:\Users\masa\source\repos\WpfApp3\WpfApp3\bin\Debug\capture.bmp"));
                dc.DrawImage(ima, rectBounds);



                // 上記で作成した描画エリア(dc)にInkCanvasのストロークを描画
                inkCanvas1.Strokes.Draw(dc);





                dc.Close();

                // ビジュアルオブジェクトをビットマップに変換する
                RenderTargetBitmap rtb = new RenderTargetBitmap(
                    (int)rectBounds.Width, (int)rectBounds.Height,
                    96, 96,
                    PixelFormats.Default);
                rtb.Render(dv);

                // ビットマップエンコーダー変数の宣言
                BitmapEncoder enc = null;

                switch (extension)
                {
                    case ".BMP":
                        enc = new BmpBitmapEncoder();
                        break;
                    case ".JPG":
                        enc = new JpegBitmapEncoder();
                        break;
                    case ".PNG":
                        enc = new PngBitmapEncoder();
                        break;
                }

                if (enc != null)
                {
                    // ビットマップフレームを作成してエンコーダーにフレームを追加する
                    enc.Frames.Add(BitmapFrame.Create(rtb));
                    // ファイルに書き込む
                    System.IO.Stream stream = System.IO.File.Create(dlgSave.FileName);
                    enc.Save(stream);
                    stream.Close();
                }

            }
        }
    }
}
