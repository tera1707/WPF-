using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

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

        private double y = 150.0;

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
            var _ = Task.Run(() =>
            {
                this.Dispatcher.Invoke(new Action(async () =>
                {
                    for (int i = 0; i < 100; i++)
                    {
                        Points.Add(new Point((double)i, y + 150.0 * Math.Sin((double)i / (Math.PI))));
                        OnPropertyChanged(nameof(Points));
                        await Task.Delay(30);
                    }
                }));
            });

        }
    }
}
