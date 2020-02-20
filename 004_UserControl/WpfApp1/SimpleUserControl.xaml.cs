using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class SimpleUserControl : UserControl
    {
        public string MyTextProp
        {
            get { return (string)GetValue(MyTextProperty); }
            set { SetValue(MyTextProperty, value); }
        }
        public static readonly DependencyProperty MyTextProperty =
            DependencyProperty.Register(
                nameof(MyTextProp),                                     // プロパティ名
                typeof(string),                                         // プロパティの型
                typeof(SimpleUserControl),                              // プロパティを所有する型＝このクラスの名前
                new PropertyMetadata("",                             // 初期値
                    new PropertyChangedCallback(StringChanged),         // プロパティが変わった時のハンドラ
                    new CoerceValueCallback(CoerceStringValue)),        // 値の矯正のためのハンドラ
                new ValidateValueCallback(ValidateStringValue));        // 値の妥当性確認のためのハンドラ

        // 値の変化 
        private static void StringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + " old : " + e.OldValue + " new : " + e.NewValue);
            // 値がかわらないとここは通らない。(プロパティに値を入れても、同じ値だとここは通らない)
        }

        // 値の矯正
        private static object CoerceStringValue(DependencyObject d, object baseValue)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + " value : " + (string)baseValue);

            var txt = (string)baseValue;
            return (txt.Length <= 5) ? txt : string.Empty;   // 5文字以上なら空文字に矯正する
        }

        // 値の妥当性確認
        private static bool ValidateStringValue(object value)
        {
            var txt = (string)value;
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + " value : " + txt); 

            if (txt == null) return false;                          // nullのときは異常(falseをreturnすると、ArgumentExceptionを返してくれる)
            if (txt.Length >= 5) throw new InvalidCastException();  // 5文字以上なら自分の好きな例外をスローしてやる
            return true;                                            // それ以外はOKとする(setされた値になる)
        }
        
        // コンストラクタ
        public SimpleUserControl()
        {
            InitializeComponent();
        }

        // テキストが変化したときのイベント
        private void MyTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);
        }
    }
}
