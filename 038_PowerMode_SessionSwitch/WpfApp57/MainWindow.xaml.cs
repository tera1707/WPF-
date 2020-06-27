using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp57
{
    /// <summary>
    /// ボタンをおしたら、サインカーブを書く
    /// </summary>
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

            // リストに表示
            var l = now.ToString("hh:mm:ss.fff ") + log;
            Logs.Insert(0, l);
            OnPropertyChanged(nameof(Logs));

            // ログファイルに書き込む
            Assembly myAssembly = Assembly.GetEntryAssembly();
            string path = Path.GetDirectoryName(myAssembly.Location) + @"\testlog.txt";
            using (var sw = new StreamWriter(path, true))
            {
                sw.Write(l + "\r\n");
            }
        }
        #endregion

        // ------------------------------------------------

        public MainWindow()
        {
            InitializeComponent();
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
        }

        private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            AddLog(e.Mode.ToString());

            switch (e.Mode)
            {
                case PowerModes.Suspend:
                    break;
                case PowerModes.Resume:
                    break;
                default:
                case PowerModes.StatusChange:
                    break;
            }
        }

        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            AddLog(e.Reason.ToString());

            switch (e.Reason)
            {
                case SessionSwitchReason.ConsoleConnect:
                    break;
                case SessionSwitchReason.ConsoleDisconnect:
                    break;
                case SessionSwitchReason.RemoteConnect:
                    break;
                case SessionSwitchReason.RemoteDisconnect:
                    break;
                default:
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddLog("Test");
        }
    }
}
