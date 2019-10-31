using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using LiveCharts;
using LiveCharts.Uwp;
using EnvSensorLibrary;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace BluetoothBleSampleUWP
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private EnvSensor EnvSensor { get; set; }
        public SeriesCollection Sc { get; set; } = new SeriesCollection();

        public MainPage()
        {
            this.InitializeComponent();

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
                    //凡例名
                    Title = "温度",
                    //系列値
                    Values = new ChartValues<double>(),
                    //線の色（省略：自動で配色されます）
                    //Stroke = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Red),
                    //直線のスムージング（0：なし、省略：あり）
                    LineSmoothness = 0,
                });
            Sc.Add(
                new LineSeries //折れ線グラフ
                {
                    //凡例名
                    Title = "湿度",
                    //系列値
                    Values = new ChartValues<double>(),
                    //線の色（省略：自動で配色されます）
                    //Stroke = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Red),
                    //直線のスムージング（0：なし、省略：あり）
                    LineSmoothness = 0,
                });
            Sc.Add(
                new LineSeries //折れ線グラフ
                {
                    //凡例名
                    Title = "照度",
                    //系列値
                    Values = new ChartValues<double>(),
                    //線の色（省略：自動で配色されます）
                    //Stroke = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Red),
                    //直線のスムージング（0：なし、省略：あり）
                    LineSmoothness = 0,
                });
            Sc.Add(
                new LineSeries //折れ線グラフ
                {
                    //凡例名
                    Title = "騒音",
                    //系列値
                    Values = new ChartValues<double>(),
                    //線の色（省略：自動で配色されます）
                    //Stroke = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Red),
                    //直線のスムージング（0：なし、省略：あり）
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

            LC_Graph.DisableAnimations = true;

            // センサーオブジェクトを作成
            EnvSensor = new EnvSensor();

            // センサー値の変化時ハンドラを設定
            EnvSensor.LatestDataChanged += (async (t, h, i, n) =>
            {
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    // 数値表示
                    this.tbTemparature.Text = ((double)t).ToString() + "℃";
                    this.tbHumidity.Text = ((double)h).ToString() + "％";
                    this.tbIlluminance.Text = ((double)i/10).ToString() + "lux";//ほんとは10で割らなくていいが、数字が大きすぎてグラフが飛び出るので一旦10で割る
                    this.tbNoise.Text = ((double)n).ToString() + "db";

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
    }
}
