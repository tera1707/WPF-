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
using System.Drawing;
using System.Globalization;

namespace WpfApp6
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

        public object Graphics { get; private set; }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            int txtboxsize = txtbox.SelectionStart;
            var fsize = MeasureString(txtbox.Text.Substring(0, txtbox.CaretIndex), txtbox.FontSize, txtbox.FontFamily.ToString());

            Console.WriteLine("キャレット位置：{0}", fsize.Width);

            Canvas.SetLeft(txtbox.Template.MyCaret, fsize.Width);
        }

        private Size MeasureString(string text, double fontSize, string typeFace)
        {
            var ft = new FormattedText(text, CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(typeFace),
                fontSize,
                Brushes.White);
            return new Size(ft.Width, ft.Height);
        }
    }
}
