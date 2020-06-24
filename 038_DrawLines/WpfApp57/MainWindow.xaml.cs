using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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

namespace WpfApp57
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // ------------------------------------------------

        public List<Point> Points
        {
            get { return _points; }
            set { _points = value; OnPropertyChanged(nameof(Points)); }
        }
        private List<Point> _points = new List<Point>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Points.Add(new Point(10, 10));
            Points.Add(new Point(100, 10));
            Points.Add(new Point(100, 100));
            Points.Add(new Point(10, 100));
            Points.Add(new Point(10, 10));
            OnPropertyChanged(nameof(Points));
        }
    }
}
