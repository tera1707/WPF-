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

namespace WpfApp1
{

    /// <summary>
    /// SimpleUserControl.xaml の相互作用ロジック
    /// https://noumenon-th.net/programming/2017/09/17/style/
    /// http://cu39.hateblo.jp/entry/20090723/1248342532
    /// https://araramistudio.jimdo.com/2016/12/01/wpf%E3%81%A7%E3%82%88%E3%81%8F%E4%BD%BF%E3%81%86xaml%E3%81%AE%E5%AE%9A%E7%BE%A9%E3%82%92%E5%88%A5%E3%83%95%E3%82%A1%E3%82%A4%E3%83%AB%E3%81%AB%E3%81%99%E3%82%8B/
    /// </summary>
    public partial class SimpleUserControl : UserControl
    {
        #region Messageプロパティ
        public string MyText
        {
            get { return (string)GetValue(MyTextProperty); }
            set { SetValue(MyTextProperty, value); }
        }

        public static readonly DependencyProperty MyTextProperty =
            DependencyProperty.Register("MyText", typeof(string), typeof(SimpleUserControl));

        public Brush MyBrush
        {
            get { return (Brush)GetValue(MyBrushProperty); }
            set { SetValue(MyBrushProperty, value); }
        }

        public static readonly DependencyProperty MyBrushProperty =
            DependencyProperty.Register("MyBrush", typeof(Brush), typeof(SimpleUserControl));
        #endregion




        public SimpleUserControl()
        {
            InitializeComponent();

        }
    }
}
