using System;
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
            Debug.WriteLine(Thread.CurrentThread.ManagedThreadId);

            Task.Run(MyTaskFunc1).Wait();

            Debug.WriteLine(Thread.CurrentThread.ManagedThreadId);

            MyTaskFunc2().Wait();

            Debug.WriteLine(Thread.CurrentThread.ManagedThreadId);
        }

        private static async Task MyTaskFunc1()
        {
            Debug.WriteLine(Thread.CurrentThread.ManagedThreadId);
            await Task.Run(() =>
            {
                Debug.WriteLine(Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(2000);
            });
            Debug.WriteLine(Thread.CurrentThread.ManagedThreadId);
        }

        private static async Task MyTaskFunc2()
        {
            Debug.WriteLine(Thread.CurrentThread.ManagedThreadId);
            await Task.Run(() =>
            {
                Debug.WriteLine(Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(2000);
            });
            Debug.WriteLine(Thread.CurrentThread.ManagedThreadId);
        }
    }
}
