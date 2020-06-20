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
            set { SetValue(MyDoubleValue1Property, value); }
        }
        public static readonly DependencyProperty MyDoubleValue1Property = DependencyProperty.Register(nameof(MyDoubleValue1), typeof(double), typeof(SimpleUserControl), new PropertyMetadata(0.0));
        
        public SimpleUserControl()
        {
            InitializeComponent();
        }
private void Button_Click(object sender, RoutedEventArgs e)
{
    MyDoubleValue1 += 1.0;
}
    }
}
