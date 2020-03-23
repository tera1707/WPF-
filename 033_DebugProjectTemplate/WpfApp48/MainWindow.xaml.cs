using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace WpfApp48
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


        #region LogFramework
        public ObservableCollection<string> Logs { get; set; } = new ObservableCollection<string>();

        public void AddLog(string log)
        {
            DateTime now = DateTime.Now;

            Logs.Add(now.ToString("hh:mm:ss.fff ") + log);
            OnPropertyChanged(nameof(Logs));
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AddLog("ログ１");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddLog("ログ２");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AddLog("ログ３");
        }
    }
}
