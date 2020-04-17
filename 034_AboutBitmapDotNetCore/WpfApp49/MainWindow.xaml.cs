using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace WpfApp49
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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

            Logs.Insert(0, now.ToString("hh:mm:ss.fff ") + log);
            OnPropertyChanged(nameof(Logs));
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Root_Loaded(object sender, RoutedEventArgs e)
        {
            AddLog("ログ");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 元になる画像を読み込む
            var uri = new Uri(@"input.jpg", UriKind.Relative);
            var image = new BitmapImage(uri);

            var msg = "表示したいstring";

            DrawingGroup drawingGroup = new DrawingGroup();
            using (var drawContent = drawingGroup.Open())
            {
                // 元の画像を先におく
                drawContent.DrawImage(image, new System.Windows.Rect(0, 0, image.PixelWidth, image.PixelHeight));
                // その上に、テキストを書き込む
                drawContent.DrawText(new FormattedText(msg, System.Globalization.CultureInfo.CurrentUICulture, System.Windows.FlowDirection.LeftToRight, new Typeface("Verdana"), 100, Brushes.Gold, VisualTreeHelper.GetDpi(this).PixelsPerDip), new System.Windows.Point(100, 100));
                drawContent.DrawRectangle(Brushes.Red, new Pen(Brushes.Black, 3), new Rect(0, 0, 200, 200));    // 四角を描く
                drawContent.DrawEllipse(Brushes.Yellow, new Pen(Brushes.Green, 3), new Point(50, 50), 10, 10);  // 丸を描く
            }

            var edittedImage = new DrawingImage(drawingGroup);

            // 画像を画面に表示
            MyImage.Source = edittedImage;

            ////////

            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawImage(edittedImage, new Rect(new Point(0, 0), new Size(edittedImage.Width, edittedImage.Height)));
            drawingContext.Close();

            RenderTargetBitmap bmp = new RenderTargetBitmap((int)edittedImage.Width, (int)edittedImage.Height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);


            var encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            using (var stream = new FileStream("output.jpeg", FileMode.Create))
            {
                encoder.Save(stream);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
