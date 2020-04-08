using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace WpfApp44
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            /////////////////////////////////////////////////////
            // 元画像をグレースケールに変換
            /////////////////////////////////////////////////////
            
            // 画像の読み込み(グレースケールに変換)
            byte[,] img = LoadImageAsGrayScale("src.jpg");
            // フィルタ用のマスク
            const int maskSize = 3;
            double[,] mask = new double[maskSize, maskSize]{
                                {1.0,1.0,1.0},
                                {1.0,-8.0,1.0},
                                {1.0,1.0,1.0}};

            // フィルタ処理(エッジ検出する。ラプラシアンフィルタというらしい)
            // エッジじゃない部分は、白色(255,255,255)に近くなって、エッジ部分は黒(0,0,0)になる
            byte[,] img2 = Filter(img, mask);

            // グレースケールになった画像を画像保存forテスト
            SaveImage(img2, "grayscale.jpg");


            /////////////////////////////////////////////////////
            // 画面表示用の、エッジ以外の部分を抜いた(透明にした)画像(WriteableBitmap)を作成
            /////////////////////////////////////////////////////

            int width = (int)img2.GetLength(0);
            int height = (int)img2.GetLength(1);
            WriteableBitmap wb = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);

            // 計算用のバイト列の準備
            int pixelsSize = (int)(width * height * 4);
            byte[] pixels = new byte[pixelsSize];

            // バイト列に色情報を入れる
            for (int i = 0; i < width * height; i++)
            {
                var val             = img2[i % width, i / width];   // その座標(ピクセル)の画素値
                pixels[4 * i]       = val;                          // blue;
                pixels[4 * i + 1]   = val;                          // green;
                pixels[4 * i + 2]   = val;                          // red;     
                
                // alpha
                if (val >= byte.MaxValue - 249) pixels[4 * i + 3] = 255;// 透過なし
                else                            pixels[4 * i + 3] = 0;  // 透過
            }

            // stride = 画像1行あたりのバイト数。今回の場合、width(=ピクセル数) * 4バイト。
            int stride = width * 4;

            // WriteableBitmapに書き込み
            wb.WritePixels(new Int32Rect(0, 0, width, height), pixels, stride, 0, 0);

            // 画面に表示
            MyImg.Source = wb;
        }


        /// ////////////////////////////////////////////////////////////////


        // 画像をグレースケール変換して読み込み
        static byte[,] LoadImageAsGrayScale(string filename)
        {
            Bitmap img = new Bitmap(filename);
            int w = img.Width;
            int h = img.Height;
            byte[,] dst = new byte[w, h];

            // bitmapクラスの画像ピクセル値を配列に挿入
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    // グレイスケールに変換
                    dst[j, i] = (byte)((img.GetPixel(j, i).R + img.GetPixel(j, i).B + img.GetPixel(j, i).G) / 3);
                }
            }
            return dst;
        }

        static System.Drawing.Bitmap SaveImage(byte[,] src, string filename)
        {
            // 画像データの幅と高さを取得
            int w = src.GetLength(0);
            int h = src.GetLength(1);
            Bitmap img = new Bitmap(w, h);
            // ピクセル値のセット
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    img.SetPixel(j, i, System.Drawing.Color.FromArgb(src[j, i], src[j, i], src[j, i]));
                }
            }

            // 画像の保存
            img.Save(filename);

            return img;
        }

        // ラプラシアンフィルタ
        static byte[,] Filter(byte[,] src, double[,] mask)
        {
            // 縦横サイズを配列から読み取り
            int w = src.GetLength(0);
            int h = src.GetLength(1);
            // マスクサイズの取得
            int masksize = mask.GetLength(0);
            // 出力画像用の配列
            byte[,] dst = new byte[w, h];

            // 画像処理
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    double sum = 0;
                    for (int k = -masksize / 2; k <= masksize / 2; k++)
                    {
                        for (int n = -masksize / 2; n <= masksize / 2; n++)
                        {
                            if (j + n >= 0 && j + n < w && i + k >= 0 && i + k < h)
                            {
                                sum += src[j + n, i + k] * mask[n + masksize / 2, k + masksize / 2];
                            }
                        }
                    }

                    dst[j, i] = DoubleToByte(sum);
                }
            }
            return dst;
        }

        // double型をbyte型に変換
        static byte DoubleToByte(double num)
        {
            if (num > 255.0) return 255;
            else if (num < 0) return 0;
            else return (byte)num;
        }
    }
}
