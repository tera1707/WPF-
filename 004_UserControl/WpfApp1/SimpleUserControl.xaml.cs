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
        public string MyTextProp
        {
            get { return (string)GetValue(MyTextProperty); }
            set { SetValue(MyTextProperty, value); }
        }
        public static readonly DependencyProperty MyTextProperty =
            DependencyProperty.Register(
                nameof(MyTextProp),                               // プロパティ名
                typeof(string),                         // プロパティの型
                typeof(SimpleUserControl),              // プロパティを所有する型＝このクラスの名前
                new PropertyMetadata("D",
                    new PropertyChangedCallback(StringChanged),
                    new CoerceValueCallback(CoerceStringValue)),
                new ValidateValueCallback(ValidateStringValue));    // 初期値

        private static void StringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // 値がかわらないとここは通らない。(プロパティに値を入れても、同じ値だとここは通らない)
            Console.WriteLine("3.PropertyChangedCallback {0} => {1}", e.OldValue, e.NewValue);

        }

        private static object CoerceStringValue(DependencyObject d, object baseValue)
        {
            var txt = (string)baseValue;

            Console.WriteLine("2.CoerceValueCallback     {0}", txt);

            if (txt.Length > 5)
            {
                txt = string.Empty;
                
            }

            //

            // UserControl内のプロパティ(ここではMyText)の値は矯正された値になるが、
            // このUserControlのMyTextにバインドされたMainWindow.xaml.csのプロパティは、値は変わらない(矯正されない)
            return txt;
        }

        private static bool ValidateStringValue(object value)
        {
            var txt = (string)value;

            Console.WriteLine("1.ValidateValueCallback   {0}", txt);
            
            // trueを返すと、セットされた値をそのまま受け入れる。
            // falseを返すと、デフォルト値に戻す。
            // 無効な値が来た際に例外を吐きたいときは、returnの代わりにExceptionをthrowしてやれば、
            // プロパティをセットしたところで例外が起きてくれる。
            // throw new InvalidCastException();
            return txt.Length <= 100;
        }

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
