using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using Windows.UI.Notifications;

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

        public static MainWindow mw; 

        public void AddLog(string log)
        {
            DateTime now = DateTime.Now;
            Logs.Add(now.ToString("hh:mm:ss.fff ") + log);
            OnPropertyChanged(nameof(Logs));
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            mw = this;

            // AUMIDを登録
            DesktopNotificationManagerCompat.RegisterAumidAndComServer<MyNotificationActivator>("MyCompany.ToastJikken");

            // COMサーバーを登録
            DesktopNotificationManagerCompat.RegisterActivator<MyNotificationActivator>();

        }
        private void Window_Loaded(object sender, RoutedEventArgs e) { }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // トーストを組み立てる
            ToastContent toastContent = new ToastContentBuilder()
                .AddToastActivationInfo("action=viewConversation&conversationId=5", ToastActivationType.Foreground)
                .AddText("Hello world!")
                .GetToastContent();

            // 組み立てたやつをもとにToastNotificationを作成
            var toast = new ToastNotification(toastContent.GetXml());

            // トーストを表示
            DesktopNotificationManagerCompat.CreateToastNotifier().Show(toast);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}