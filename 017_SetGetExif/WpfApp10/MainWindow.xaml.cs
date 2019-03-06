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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

/// <summary>
/// Exif規格の資料
/// http://www.cipa.jp/std/documents/j/DC-008-2012_J.pdf
/// わかりやすいExif規格の説明
/// http://dsas.blog.klab.org/archives/52123322.html
/// Metadataの列挙の仕方
/// https://social.msdn.microsoft.com/Forums/netframework/ja-JP/fb7c0993-9158-4ae1-af7d-d12c879b1ad1/bitmapmetadata0thifd?forum=netfxgeneralja
/// サンプル
/// http://funct.hatenablog.com/entry/20151007/1444231916
/// 
/// 
/// </summary>
namespace WpfApp10
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            tb_FileName.Text = @"C:\Users\masa\Desktop\IMG_5050.JPG";
        }

        /// <summary>
        /// ファイルを開くボタン
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
        /// 画像のMetaDataの読み込み方
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // ファイル読み込み
            Uri uri = new Uri(tb_FileName.Text, UriKind.Absolute);
            BitmapFrame frame = BitmapFrame.Create(uri);

            // Metadataを格納し、下で取り出す
            BitmapMetadata metadata = frame.Metadata as BitmapMetadata;

            ////////////////////
            // 取り出し方① ContainsQueryでそのデータがあるか確認し、あればGetQueryで取り出す
            ////////////////////

            // ここのvarの値の型をbreak張って見て、uintであってるかどうか確認する！
            // また、戻り値の型が何かを確認する。
            // また、このクエリは画像のフォーマットによるらしい
            var queryMakerNote = string.Format("/app1/ifd/exif/{{uint={0}}}", 0x9000);

            bool containsMakerNote = metadata.ContainsQuery(queryMakerNote);
            if (containsMakerNote)
            {
                var objMakerNote = metadata.GetQuery(queryMakerNote);
                // GetQueryで帰ってくるデータはobject型なので、適切な型にキャストする必要がある
                //byte[] dataMakerNote = objMakerNote.GetBlobValue();
                // これがMakerNoteのデータ
                Console.WriteLine("Type : " +objMakerNote.GetType());
            }

            ////////////////////
            // 取り出し方② いきなりGetQueryで取り出す。{uint=xxxx}のところの型が間違えていると、エラーとなり取り出せない
            ////////////////////

            var val1 = metadata.GetQuery("/app1/ifd/gps/subifd:{ushort=1}");
            Debug.WriteLine(val1);
            var val2 = metadata.GetQuery("/app1/ifd/gps/subifd:{ushort=2}");
            Debug.WriteLine(val2);
            var val3 = metadata.GetQuery("/app1/ifd/gps/subifd:{ushort=3}");
            Debug.WriteLine(val3);
            var val4 = metadata.GetQuery("/app1/ifd/gps/subifd:{ushort=4}");
            Debug.WriteLine(val4);
        }

        /// <summary>
        /// 読み込み位置の指定(クエリ)の実験
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // ファイル/Metadata読み込み
            Uri uri = new Uri(tb_FileName.Text, UriKind.Absolute);
            BitmapFrame frame = BitmapFrame.Create(uri);
            BitmapMetadata metadata = frame.Metadata as BitmapMetadata;
            
            BitmapMetadata metadata2 = (BitmapMetadata)metadata.GetQuery("/app1");
            Debug.WriteLine("Metadataを列挙 /app1");
            foreach (string str in metadata2)
            {
                Debug.WriteLine(str);
            }
#if true
            BitmapMetadata metadata4 = (BitmapMetadata)metadata.GetQuery("/app1/ifd");
            Debug.WriteLine("Metadataを列挙 /app1/ifd");
            foreach (string str in metadata4)
            {
                Debug.WriteLine(str);
            }
#else
            // 以下は、上の処理と同じ
            BitmapMetadata metadata4 = (BitmapMetadata)metadata.GetQuery("/app1/{ushort=0}");
            Debug.WriteLine("Metadataを列挙 /app1/{ushort=0}");
#endif
#if true
            BitmapMetadata metadata6 = (BitmapMetadata)metadata.GetQuery("/app1/ifd/exif");
            Debug.WriteLine("Metadataを列挙 /app1/ifd/exif");
            foreach (string str in metadata6)
            {
                Debug.WriteLine(str);
            }
#else
            // 以下は、上の処理と同じ
            BitmapMetadata metadata6 = (BitmapMetadata)metadata.GetQuery("/app1/ifd/{ushort=34665}");
            Debug.WriteLine("Metadataを列挙 /app1/ifd/{ushort=34665}");
#endif
#if true
            BitmapMetadata metadataGPS = (BitmapMetadata)metadata.GetQuery("/app1/ifd/gps");
            Debug.WriteLine("Metadataを列挙 /app1/ifd/gps");
            foreach (string str in metadataGPS)
            {
                Debug.WriteLine(str);
            }
#else
            // 以下は、上の処理と同じ
            BitmapMetadata metadataGPS = (BitmapMetadata)metadata.GetQuery("/app1/ifd/{ushort=34853}");
            Debug.WriteLine("Metadataを列挙 /app1/ifd/{ushort=34853}");
#endif
        }

        /// <summary>
        /// EXIF情報表示の例
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // ファイル/Metadata読み込み
            Uri uri = new Uri(tb_FileName.Text, UriKind.Absolute);
            BitmapFrame frame = BitmapFrame.Create(uri);
            BitmapMetadata metadata = frame.Metadata as BitmapMetadata;

            // GetQueryしてエラーになる場合、
            // クエリの中の型(「ushort」の部分)が間違っているかもしれない。
            // その時は、「読み込み位置の指定(クエリ)の実験」で行ったMetadataの列挙のところで
            // 出力される型を確認する。(画像ファイルによって、型が異なる？)

            var GPSLatitudeRef = metadata.GetQuery("/app1/ifd/gps/subifd:{ushort=1}");  // 北緯or南緯
            var GPSLatitude = metadata.GetQuery("/app1/ifd/gps/subifd:{ushort=2}");     // 緯度
            var GPSLongitudeRef = metadata.GetQuery("/app1/ifd/gps/{ushort=3}");        // 東経or西経
            var GPSLongitude = metadata.GetQuery("/app1/ifd/gps/{ushort=4}");           // 経度

            var Maker = metadata.GetQuery("/app1/ifd/{ushort=271}");                    // メーカー名
            var Model = metadata.GetQuery("/app1/ifd/{ushort=272}");                    // モデル名

            var MakerExif = metadata.GetQuery("/app1/ifd/exif/{ushort=34864}");         // Exifバージョン
        }
    }
}
