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

        // 何のイベントハンドラが動いたのか、またイベントのArgの中身を表示する。
        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            SystemEvents.SessionSwitch          += ((sender, e) => { AddLog("SessionSwitch       :" + e.Reason.ToString()); });
            SystemEvents.SessionEnding          += ((sender, e) => { AddLog("SessionEnding       :" + e.Reason.ToString()); });
            SystemEvents.SessionEnded           += ((sender, e) => { AddLog("SessionEnded        :" + e.Reason.ToString()); });
            SystemEvents.PowerModeChanged       += ((sender, e) => { AddLog("PowerModeChanged    :" + e.Mode.ToString()); });
            SystemEvents.EventsThreadShutdown   += ((sender, e) => { AddLog("EventsThreadShutdown:" + e.ToString()); });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddLog(MethodBase.GetCurrentMethod().Name);
            AddLog("Test");
        }
    }
}
