using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp13
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // 基本
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var task1 = Task.Run(() => 
            {
                Thread.Sleep(3000);
                Debug.WriteLine("task1 完了");
            });

            var task2 = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Debug.WriteLine("task2 完了");
            });

            var task3 = Task.Run(() =>
            {
                Thread.Sleep(2000);
                Debug.WriteLine("task3 完了");
            });

            await Task.WhenAll(task1, task2, task3);
            Debug.WriteLine("task1,2,3 完了");
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var task1 = Task.Run(() => LongWaitingMethod(3000)); // task1 開始
            var task2 = Task.Run(() => LongWaitingMethod(1000)); // task2 開始
            var task3 = Task.Run(() => LongWaitingMethod(2000)); // task3 開始

            await Task.WhenAll(task1, task2, task3);
            Debug.WriteLine("task1,2,3 完了");
        }

        // 重い処理の代わりのメソッド
        private void LongWaitingMethod(int millisec)
        {
            Thread.Sleep(millisec);
            Debug.WriteLine("LongWaitingMethod " + millisec + " 完了");
        }

        // Task.WhenAllの前に、すでに全部のtaskが終わっている
        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var task1 = Task.Run(() => LongWaitingMethod(3000)); // task1 開始
            var task2 = Task.Run(() => LongWaitingMethod(1000)); // task2 開始
            var task3 = Task.Run(() => LongWaitingMethod(2000)); // task3 開始

            // 5秒ここで待つ(ブロック)
            LongWaitingMethod(5000);
            Debug.WriteLine("5秒待ち 終わり");

            // task1-3はすでに完了しているので、ここは即抜ける
            await Task.WhenAll(task1, task2, task3);
            Debug.WriteLine("task1,2,3 完了");
        }

        // 戻り値ありのTask(全部同じ型の戻り値)
        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var task1 = Task<string>.Run(() => LongWaitingMethodReturnString(3000)); // task1 開始
            var task2 = Task<string>.Run(() => LongWaitingMethodReturnString(1000)); // task2 開始
            var task3 = Task<string>.Run(() => LongWaitingMethodReturnString(2000)); // task3 開始
            
            string[] ret = await Task.WhenAll(task1, task2, task3);
            Debug.WriteLine("task1,2,3 完了");

            // 戻り値
            Debug.WriteLine("戻り値1：" + ret[0]);
            Debug.WriteLine("戻り値2：" + ret[1]);
            Debug.WriteLine("戻り値3：" + ret[2]);
        }

        private string LongWaitingMethodReturnString(int millisec)
        {
            Thread.Sleep(millisec);
            Debug.WriteLine("LongWaitingMethod " + millisec + " 完了");
            return "LongWaitingMethod " + millisec + " 完了";
        }

        // 戻り値ありのTask(全部異なる型の戻り値)
        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var task1 = Task<string>.Run(() => { Thread.Sleep(5000); return "task1"; });    // task1 開始
            var task2 = Task<int>.Run(() => { Thread.Sleep(1000); return 2; });             // task2 開始
            var task3 = Task<double>.Run(() => { Thread.Sleep(2000); return 3.33; });       // task3 開始

            // 戻り値
            Debug.WriteLine("戻り値1：" + task1.Result); // ここでブロックかかり、task1が終わるまでの5秒間UIフリーズ
            Debug.WriteLine("戻り値2：" + task2.Result);
            Debug.WriteLine("戻り値3：" + task3.Result);

            await Task.WhenAll(task1, task2, task3);
            Debug.WriteLine("task1,2,3 完了");

        }
    }
}
