using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace WpfApp48
{
    [ClassInterface(ClassInterfaceType.None)]
    [ComSourceInterfaces(typeof(INotificationActivationCallback))]
    [Guid("EF608355-E10B-487C-BA55-AE7E400E4EC7"), ComVisible(true)]
    class MyNotificationActivator : NotificationActivator
    {
        public override void OnActivated(string invokedArgs, NotificationUserInput userInput, string appUserModelId)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                OpenWindowIfNeeded();

                // ログ表示
                MainWindow.mw.AddLog("OnActivated()実行しました");
                MainWindow.mw.AddLog(invokedArgs);

            });
        }

        private void OpenWindowIfNeeded()
        {
            MainWindow.mw.AddLog("OpenWindowIfNeeded()");

            // ウインドウを開く (アプリが閉じている間にトーストが押されたとき(≒アクションセンターで押された時)等)
            if (App.Current.Windows.Count == 0)
            {
                MainWindow.mw.AddLog("メインウインドウ表示");
                new MainWindow().Show();
            }

            // ウインドウをActivateして、フォーカスをあてる
            MainWindow.mw.AddLog("メインウインドウにフォーカス当てました");
            App.Current.Windows[0].Activate();

            // 最小化してたら通常の大きさに戻す
            App.Current.Windows[0].WindowState = WindowState.Normal;
        }
    }
}
