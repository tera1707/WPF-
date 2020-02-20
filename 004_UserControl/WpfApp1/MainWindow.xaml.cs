using System;
using System.ComponentModel;
using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // userControlにバインドして文字列を渡すためのプロパティ
        public string DispText
        {
            get { return _dispText; }
            set { Console.WriteLine("DispText = {0}", value); _dispText = value; OnPropertyChanged(nameof(DispText));  }
        }
        private string _dispText = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // "A"を付け足していく
                DispText += "A";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
