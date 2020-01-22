using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WMPLib;

namespace WpfApp38
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// ルーレット回転中かどうか
        /// </summary>
        bool IsRounding = false;

        /// <summary>
        /// メンバー一覧
        /// ここにメンバー名をAddしたら、
        /// Window_Loaded()の中で枠を自動でつくる
        /// </summary>
        List<string> Members = new List<string>();

        /// <summary>
        /// 音声再生
        /// </summary>
        WindowsMediaPlayer _mediaPlayer = new WindowsMediaPlayer();

        /// <summary>
        /// ドラムロール音
        /// (ニコニ・コモンズの素材ライブラリより)
        /// https://commons.nicovideo.jp/materials/
        /// </summary>
        string SoundDrumRoll = @".\sound\nc90552.mp3";

        string SoundSymbal = @".\sound\nc166146.wav";

        /// <summary>
        /// コンストラクタ
        /// ここでメンバー登録をする
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // jsonから名前リストを読み出し
            var namelist = ReadSettingJson();

            foreach (var name in namelist.Names)
            {
                Members.Add(name);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // メンバー一覧を、起動時毎回ランダムに並び替え
            Members = Members.OrderBy(a => Guid.NewGuid()).ToList();

            // 一人あたりの使用する角度を決める
            int anglePerOne = 360 / Members.Count;

            // 人数分の線を引き、名前のテキストを作成する
            for (int i = 0; i < Members.Count; i++)
            {
                ////////////////////////
                // 人数分の区切り線を引く
                ////////////////////////
                var tfgLine = new TransformGroup();
                tfgLine.Children.Add(new RotateTransform(i * anglePerOne));

                var line = new Line()
                {
                    X1 = 0,
                    Y1 = 0,
                    X2 = 0,
                    Y2 = RouletteEllipse.Width / 2,
                    StrokeThickness = 5,
                    Stroke = Brushes.Red,
                    Fill = Brushes.Transparent,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    RenderTransformOrigin = new Point(0, 1.0),
                    RenderTransform = tfgLine
                };

                RouletteMain.Children.Add(line);

                ////////////////////////
                // 人数分の名前を書く
                ////////////////////////
                int textHeight = 30;
                var tfgText = new TransformGroup();
                tfgText.Children.Add(new RotateTransform(-90 + (anglePerOne / 2) + (i * anglePerOne)));

                var text = new TextBlock()
                {
                    Text = Members[i],
                    Width = RouletteEllipse.Width / 2,                  // ルーレットの円の半分
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    TextAlignment = TextAlignment.Center,
                    FontSize = textHeight,
                    RenderTransformOrigin = new Point(0, 0.5),
                    RenderTransform = tfgText
                };

                RouletteMain.Children.Add(text);
            }
        }

        /// <summary>
        /// ルーレットのスタート/ストップ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // ルーレットを回すためのStoryBoardを検索
            var sb = FindResource("StartRoulettea") as Storyboard;
            var sbArrow = FindResource("StartRouletReverse") as Storyboard;

            if (IsRounding == false)
            {
                // 回転開始(スタート)
                sb.Begin();
                sbArrow.Begin();

                StartButton.Content = "ストップ";

                // 音声再生(有効時のみ)
                if (SoundEnable.IsChecked == true)
                {
                    _mediaPlayer.URL = SoundDrumRoll;
                    _mediaPlayer.controls.play();
                }
                SoundEnable.IsEnabled = false;
            }
            else
            {
                // 回転停止(ストップ)
                sb.Pause();
                sbArrow.Pause();

                StartButton.Content = "スタート";

                // 音声再生(有効時のみ)
                if (SoundEnable.IsChecked == true)
                {
                    _mediaPlayer.URL = SoundSymbal;
                    _mediaPlayer.controls.play();
                }
                SoundEnable.IsEnabled = true;
            }

            IsRounding = !IsRounding;
        }

        /// <summary>
        /// クリップボードにコピーボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var canvas = new RenderTargetBitmap((int)RouletteWhole.ActualWidth, (int)RouletteWhole.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            canvas.Render(RouletteWhole);

            Clipboard.SetImage(canvas);
        }

        /// <summary>
        /// json読み出し
        /// </summary>
        /// <returns></returns>
        private NameListJson ReadSettingJson()
        {
            // デシリアライズ(jsonファイル→クラスオブジェクト)
            string jsonFilePath = @".\settings\NameList.json";
            var data = new NameListJson();

            using (var ms = new FileStream(jsonFilePath, FileMode.Open))
            {
                var serializer = new DataContractJsonSerializer(typeof(NameListJson));
                data = (NameListJson)serializer.ReadObject(ms);
            }

            return data;
        }

    }

    /// <summary>
    /// jsonから読み出した名前リストを保存するクラス
    /// ※utf-8
    /// </summary>
    [DataContract]
    public class NameListJson
    {
        [DataMember]
        public string[] Names { get; set; }
    }

}
