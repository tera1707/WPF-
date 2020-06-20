using System;
using System.ComponentModel;
using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>  this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public double DoubleValue1
        {
            get { return _doubleValue1; }
            set { Console.WriteLine("* MainWindow DoubleValue1 = {0} ", value); _doubleValue1 = value; OnPropertyChanged(nameof(DoubleValue1)); }
        }
        private double _doubleValue1 = 0.0;
        public double DoubleValue2
        {
            get { return _doubleValue2; }
            set { Console.WriteLine("* MainWindow DoubleValue2 = {0} ", value); _doubleValue2 = value; OnPropertyChanged(nameof(DoubleValue2)); }
        }
        private double _doubleValue2 = 0.0;
        public double DoubleValue3
        {
            get { return _doubleValue3; }
            set { Console.WriteLine("* MainWindow DoubleValue3 = {0} ", value); _doubleValue3 = value; OnPropertyChanged(nameof(DoubleValue3)); }
        }
        private double _doubleValue3 = 0.0;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DoubleValue1++;
            DoubleValue2++;
            DoubleValue3++;
        }
    }
}
