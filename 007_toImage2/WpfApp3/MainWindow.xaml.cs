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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string path = @"C:\Users\masa\source\repos\006_toImage\WpfApp3\bin\Debug\out";
            string path1 = path + "1.bmp";
            string path2 = path + "2.bmp";
            string path3 = path + "3.bmp";

            //myCanvas.toImage(path);

            Canvas canvas = new Canvas();

            Image img = new Image() { Width = myImage.Width, Height = myImage.Height };
            img.Source = myImage.toImage(path1);

            Image inkcanvas = new Image() { Width = myInkCanvas.Width, Height = myInkCanvas.Height };
            inkcanvas.Source = myInkCanvas.toImage(path2);

            canvas.Width = img.Width;
            canvas.Height = img.Height;
            canvas.Children.Add(img);
            canvas.Children.Add(inkcanvas);

            canvas.toImage(path3);

        }
    }
}
