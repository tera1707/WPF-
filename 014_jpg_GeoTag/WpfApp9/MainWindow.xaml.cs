using System;
using System.Windows;
using Windows.Devices.Geolocation;      // BasicGeoposition、Geopointのため
using Windows.Storage.FileProperties;   // GeotagHelperのため
using Microsoft.Win32;                  // ファイルダイアログのため
using System.IO;
using System.Collections.Generic;

// UWPのAPIを使用するため、プロジェクトの参照に下記の追加必要
// C:\Program Files (x86)\Windows Kits\10\UnionMetadata\Windows.winmd
// C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework.NETCore\v4.5\System.Runtime.WindowsRuntime.dll

namespace WpfApp9
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

        /// <summary>
        /// ファイル選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "";
            ofd.DefaultExt = "*.jpg";
            if (ofd.ShowDialog() == true)
            {
                FilePathBox.Text = ofd.FileName;
            }
        }

        /// <summary>
        /// 情報読み出し
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var filepath = FilePathBox.Text;
            var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(filepath);
            if (file != null)
            {
                var gps = await GeotagHelper.GetGeotagAsync(file);

                if (gps != null)
                {
                    MessageBox.Show("latitude : " + gps.Position.Latitude + "\r\nLongitude : " + gps.Position.Longitude);
                }
                else
                {
                    // gpsデータなし
                }
            }
        }

        /// <summary>
        /// 情報書き込み
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var cur = Directory.GetCurrentDirectory();      // exeのあるディレクトリ
            var filepath = cur + @"\ginga.bmp";             // 元の画像
            var filepath_out_jpg = cur + @"\ginga_out.jpg"; // jpgとして保存する画像
            var filepath_out_bmp = cur + @"\ginga_out_fake.jpg"; // bmpとして保存する画像

            using (var fs = new FileStream(filepath, FileMode.Open, FileAccess.ReadWrite))
            using (var bmp = new System.Drawing.Bitmap(fs))
            {
                // 元の画像を、jpgとbmpで保存し分ける
                bmp.Save(filepath_out_jpg, System.Drawing.Imaging.ImageFormat.Jpeg);
                bmp.Save(filepath_out_bmp, System.Drawing.Imaging.ImageFormat.Bmp);
            }

            // GPS値作成
            BasicGeoposition bgps = new BasicGeoposition() { Latitude = 3.0, Longitude = 2.0, Altitude = 1.0 };
            // GPS値をGeopointにセット
            Geopoint gps = new Geopoint(bgps);

            try
            {
                // GPS値をjpgファイルに書き込み
                var stjpg = await Windows.Storage.StorageFile.GetFileFromPathAsync(filepath_out_jpg);
                await GeotagHelper.SetGeotagAsync(stjpg, gps);// →こっちは問題なくgeotag付与できる
                var stbmp = await Windows.Storage.StorageFile.GetFileFromPathAsync(filepath_out_bmp);
                await GeotagHelper.SetGeotagAsync(stbmp, gps);// →こっちは、jpgではないのでgeotag付与時に例外発生
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
