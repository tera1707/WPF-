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
        /// <summary>
        /// 文字列の依存関係プロパティ
        /// </summary>
        public string MyText
        {
            get { return (string)GetValue(MyTextProperty); }
            set { SetValue(MyTextProperty, value); }
        }
        public static readonly DependencyProperty MyTextProperty =
            DependencyProperty.Register(
                "MyText",                               // プロパティ名
                typeof(string),                         // プロパティの型
                typeof(SimpleUserControl),              // プロパティを所有する型＝このクラスの名前
                new PropertyMetadata(string.Empty));    // 初期値

        /// <summary>
        /// コマンドの依存関係プロパティ
        /// </summary>
        public ICommand MyCommand
        {
            get { return (ICommand)GetValue(MyCommandProperty); }
            set { SetValue(MyCommandProperty, value); }
        }
        public static readonly DependencyProperty MyCommandProperty =
            DependencyProperty.Register(
                "MyCommand",                    // プロパティ名
                typeof(ICommand),               // プロパティの型
                typeof(SimpleUserControl),      // プロパティを所有する型＝このクラスの名前
                new PropertyMetadata(null));    // 初期値

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SimpleUserControl()
        {
            InitializeComponent();
        }
    }
}
