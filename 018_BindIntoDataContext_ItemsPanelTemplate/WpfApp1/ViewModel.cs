using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1
{
    class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // ItemsControlのSourceに設定するコレクション
        public ObservableCollection<Polyline> Pl { get { return pl; } }
        public ObservableCollection<Polyline> pl = new ObservableCollection<Polyline>();
        
        // DataTemplateの中にバインドしたいプロパティ
        public Thickness MyThickness { get; } = new Thickness(20);

        public ViewModel()
        {
            Polyline pl1 = new Polyline();
            pl1.Points.Add(new Point(10, 10));
            pl1.Points.Add(new Point(200, 150));
            pl1.Stroke = new SolidColorBrush(Colors.Red);
            pl1.StrokeThickness = 5;
            pl.Add(pl1);

            Polyline pl2 = new Polyline();
            pl2.Points.Add(new Point(15, 15));
            pl2.Points.Add(new Point(250, 600));
            pl2.Points.Add(new Point(600, 400));
            pl2.Stroke = new SolidColorBrush(Colors.Yellow);
            pl2.StrokeThickness = 5;
            pl.Add(pl2);
        }
    }
}
