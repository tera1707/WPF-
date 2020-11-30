using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Windows;

namespace WpfApp48
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)=> this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        // Mutexの名前        
        // 「Global\\」をつけると、自分以外のUserとも共有できるMutexになる
        // ただし、Create時に振るアクセスできるようにしておかないと、別Userが
        // Create時にアクセス拒否例外になる
        string mutexName = "Global\\MyMutex";
        Mutex mutex;

        public MainWindow() => InitializeComponent();
        private void Window_Loaded(object sender, RoutedEventArgs e) { }

        // Mutex作成
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (mutex == null)
            {

                try
                {
                    if (cbSecurity.IsChecked != false)
                    {
                        var mutexSecurity = new MutexSecurity();
                        mutexSecurity.AddAccessRule(
                          new MutexAccessRule(
                            new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                            MutexRights.Synchronize | MutexRights.Modify,
                            AccessControlType.Allow
                          )
                        );
                        mutex = new Mutex(false, mutexName, out _, mutexSecurity);
                        AddLog("MyMutex作成OK(フルアクセス)");
                    }
                    else
                    {
                        mutex = new Mutex(false, mutexName, out _, null);
                        AddLog("MyMutex作成OK(通常アクセス)");
                    }
                }
                catch (Exception ex)
                {
                    AddLog(ex.Message);
                }
            }
            else
            {
                AddLog("MyMutexすでに作成済み");
            }
        }

        // チェック
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            bool signal = false;

            if (mutex == null)
            {
                AddLog("MyMutex未作成");
                return;
            }
            else
            {
                AddLog("MyMutex WaitOne()実行");
            }

            try
            {
                // mutexの所有権を要求する
                // C++のCreateMutex()とOpenMutex()を一緒にやる感じ
                signal = mutex.WaitOne(5000);
            }
            catch (AbandonedMutexException ex)
            {
                // 相手がmutexを解放する前に終了してしまった場合
                signal = true;
                AddLog(ex.Message);
            }

            if (signal)
            {
                AddLog("MyMutex作成完了");
            }
            else
            {
                AddLog("MyMutexタイムアウト");
                mutex = null;
            }
        }

        // 解放
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (mutex != null)
            {
                AddLog("Mutex解放します");
                mutex.ReleaseMutex();
                mutex.Close();
                mutex = null;
            }
            else
            {
                AddLog("MutexすでにReleaseしてます");
            }
        }
    }
}