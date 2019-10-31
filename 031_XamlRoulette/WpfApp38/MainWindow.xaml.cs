using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

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
        /// コンストラクタ
        /// ここでメンバー登録をする
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            Members.Add("Aさん");
            Members.Add("Bさん");
            Members.Add("Cさん");
            Members.Add("Dさん");
            Members.Add("Eさん");
            Members.Add("Fさん");
            Members.Add("Gさん");
            Members.Add("Hさん");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
            
            if (IsRounding == false)
            {
                // 回転開始(スタート)
                sb.Begin();

                StartButton.Content = "ストップ";
            }
            else
            {
                // 回転停止(ストップ)
                sb.Pause();

                StartButton.Content = "スタート";
            }

            IsRounding = !IsRounding;
        }
    }
}
