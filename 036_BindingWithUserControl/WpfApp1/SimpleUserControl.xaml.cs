using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class SimpleUserControl : UserControl
    {
        public double MyDoubleValue1
        {
            get => (double)GetValue(MyDoubleValue1Property);
            set { Console.WriteLine(" - UserControl DoubleValue1 = {0} ", value); SetValue(MyDoubleValue1Property, value); }
        }
        public static readonly DependencyProperty MyDoubleValue1Property = DependencyProperty.Register(nameof(MyDoubleValue1), typeof(double), typeof(SimpleUserControl), new PropertyMetadata(0.0));
        //---------------------------------------------------------------------------
        public double MyDoubleValue2
        {
            get => (double)GetValue(MyDoubleValue2Property);
            set { Console.WriteLine(" - UserControl DoubleValue2 = {0} ", value); SetValue(MyDoubleValue2Property, value); }
        }
        public static readonly DependencyProperty MyDoubleValue2Property = DependencyProperty.Register(nameof(MyDoubleValue2), typeof(double), typeof(SimpleUserControl), new PropertyMetadata(0.0));
        //---------------------------------------------------------------------------
        public double MyDoubleValue3
        {
            get => (double)GetValue(MyDoubleValue3Property);
            set { Console.WriteLine(" - UserControl DoubleValue3 = {0} ", value); SetValue(MyDoubleValue3Property, value); }
        }
        public static readonly DependencyProperty MyDoubleValue3Property = DependencyProperty.Register(nameof(MyDoubleValue3), typeof(double), typeof(SimpleUserControl), new PropertyMetadata(0.0));
        //---------------------------------------------------------------------------
        // コンストラクタ
        public SimpleUserControl()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MyDoubleValue1 += 1.0;
            MyDoubleValue2 += 1.0;
            MyDoubleValue3 += 1.0;
        }
    }
}
