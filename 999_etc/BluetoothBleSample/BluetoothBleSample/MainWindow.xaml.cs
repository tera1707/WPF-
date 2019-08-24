using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;
using LiveCharts;
using LiveCharts.Wpf;
using EnvSensorLibrary;

namespace BluetoothBleSample
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {

        private EnvSensor EnvSensor { get; set; }

        public SeriesCollection Sc { get; set; } = new SeriesCollection();

        private int _count = 0;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            // グラフにはLiveChartを使用
            // https://qiita.com/myasu/items/e8980be544761d668a82

            /////////////////////////////////////
            //ステップ１：系列にグラフを追加
            /////////////////////////////////////
            Sc.Clear();
            Sc.Add(
                new LineSeries //折れ線グラフ
                {
                    Title = "温度",
                    Values = new ChartValues<double>(),
                    LineSmoothness = 0,
                });
            Sc.Add(
                new LineSeries //折れ線グラフ
                {
                    Title = "湿度",
                    Values = new ChartValues<double>(),
                    LineSmoothness = 0,
                });
            Sc.Add(
                new LineSeries //折れ線グラフ
                {
                    Title = "照度",
                    Values = new ChartValues<double>(),
                    LineSmoothness = 0,
                });
            Sc.Add(
                new LineSeries //折れ線グラフ
                {
                    Title = "騒音",
                    Values = new ChartValues<double>(),
                    LineSmoothness = 0,
                });


            /////////////////////////////////////
            //ステップ２:LiveChartの設定
            /////////////////////////////////////
            //凡例の表示位置
            LC_Graph.LegendLocation = LegendLocation.Right;

            //軸の設定
            LC_Graph.AxisX.Clear();     //デフォルトで設定されている軸をクリア
            LC_Graph.AxisX.Add(new Axis { Title = "横軸", FontSize = 20 });
            LC_Graph.AxisY.Clear();
            LC_Graph.AxisY.Add(new Axis { Title = "縦軸", FontSize = 20 });


            // センサーオブジェクトを作成
            EnvSensor = new EnvSensor();

            // センサー値の変化時ハンドラを設定
            EnvSensor.LatestDataChanged += ((t, h, i, n) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.tbTemparature.Text = ((double)t).ToString();
                    this.tbHumidity.Text = ((double)h).ToString();
                    this.tbIlluminance.Text = ((double)i).ToString();
                    this.tbNoise.Text = ((double)n).ToString();

                    // グラフ表示
                    Sc[0].Values.Add(t);
                    Sc[1].Values.Add(h);
                    Sc[2].Values.Add(i);
                    Sc[3].Values.Add(n);

                    // データが60件ある場合は古いほうから削除する
                    if (Sc[0].Values.Count > 60)
                    {
                        Sc[0].Values.RemoveAt(0);
                        Sc[1].Values.RemoveAt(0);
                        Sc[2].Values.RemoveAt(0);
                        Sc[3].Values.RemoveAt(0);
                    }
                });
            });

            // センサーと通信開始
            EnvSensor.StartCommunicationWithEnvSensorBL01();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}