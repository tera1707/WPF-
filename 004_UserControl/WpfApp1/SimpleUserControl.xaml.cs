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
    /// </summary>
    public partial class SimpleUserControl : UserControl
    {
        #region Messageプロパティ
        public string MyText
        {
            get
            {
                return (string)GetValue(MyTextProperty);
            }
            set
            {
                SetValue(MyTextProperty, value);
            }
        }

        public static readonly DependencyProperty MyTextProperty =
            DependencyProperty.Register("MyText", typeof(string), typeof(SimpleUserControl), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion




        public SimpleUserControl()
        {
            InitializeComponent();

        }
    }
}
