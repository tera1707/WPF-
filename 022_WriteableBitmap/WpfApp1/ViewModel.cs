using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace WpfApp1
{
    class ViewModel : BindingBase
    {
        public WriteableBitmap MyWBitmap { get; set; } = new WriteableBitmap(1280, 720, 96, 96, PixelFormats.Pbgra32, null);

        DispatcherTimer _drawingTimer = new DispatcherTimer();

        public ViewModel()
        {
            // 描画タイマー作動
            _drawingTimer.Interval = new TimeSpan(0, 0, 0, 0, 33);
            _drawingTimer.Tick += _timer_draw_Tick;
            _drawingTimer.Start();
        }

        // 画面の更新はメインスレッドで実施しないといけないので、Task等ではできない。
        // (具体的には、MyWBitmapに値を入れるところを別スレッドにはできない)
        private void _timer_draw_Tick(object sender, EventArgs e)
        {
            int width = (int)1280;
            int height = (int)720;

            // 計算用のバイト列の準備
            int pixelsSize = (int)(width * height * 4);
            byte[] pixels = new byte[pixelsSize];

            Random rnd = new System.Random();    // インスタンスを生成
            int rndMax = 256;                    // 0～256の乱数を取得

            // バイト列に色情報を入れる
            for (int i = 0; i < width * height; i++)
            {
                pixels[4 * i] = (byte)rnd.Next(rndMax);        //blue;
                pixels[4 * i + 1] = (byte)rnd.Next(rndMax);    // green;
                pixels[4 * i + 2] = (byte)rnd.Next(rndMax);    // red;
                pixels[4 * i + 3] = (byte)255;                 //alpha
            }

            // バイト列をBitmapImageに変換する
            int stride = width * 4; // 一行あたりのバイトサイズ(Pbgra32だから、1ピクセル当たり4バイトとなるので、ピクセル数に4を掛ける)
            MyWBitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, stride, 0, 0);
        }
    }
}