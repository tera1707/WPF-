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
using OxyPlot;
using EnvSensorLibrary;

namespace BluetoothBleSample
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<DataPoint> DataList { get; }

        private EnvSensor EnvSensor { get; set; }

        private int _count = 0;

        public MainWindow()
        {
            InitializeComponent();

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

                    DataList.Add(new DataPoint(_count++, t));
                    graph.InvalidatePlot();
                });
            });

            // センサーと通信開始
            EnvSensor.StartCommunicationWithEnvSensorBL01();

            // グラフ
            DataList = new List<DataPoint>();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}