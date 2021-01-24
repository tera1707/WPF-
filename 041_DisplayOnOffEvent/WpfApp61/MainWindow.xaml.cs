using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace WpfApp61
{
    public partial class MainWindow : Window
    {
        #region RegisterPowerSettingNotificationのための準備部分
        private const int WM_POWERBROADCAST = 0x0218;
        private const int PBT_POWERSETTINGCHANGE = 0x8013;
        private static Guid GUID_CONSOLE_DISPLAY_STATE = new Guid(0x6fe69556, 0x704a, 0x47a0, 0x8f, 0x24, 0xc2, 0x8d, 0x93, 0x6f, 0xda, 0x47);
        private static Guid GUID_MONITOR_POWER_ON = new Guid("02731015-4510-4526-99E6-E5A17EBD1AEA");
        private static Guid GUID_BATTERY_PERCENTAGE_REMAINING = new Guid("A7AD8041-B45A-4CAE-87A3-EECBB468A9E1");
        const int DEVICE_NOTIFY_WINDOW_HANDLE = 0x00000000;

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct POWERBROADCAST_SETTING
        {
            public Guid PowerSetting;
            public uint DataLength;
            public byte Data;
        }

        [DllImport(@"User32.dll", SetLastError = true, EntryPoint = "RegisterPowerSettingNotification", CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr RegisterPowerSettingNotification(IntPtr hRecipient, ref Guid PowerSettingGuid, uint Flags);

        [DllImport(@"User32.dll", EntryPoint = "UnregisterPowerSettingNotification", CallingConvention = CallingConvention.StdCall)]
        static extern bool UnregisterPowerSettingNotification(IntPtr RegistrationHandle);

        #endregion

        private IntPtr registerConsoleDisplayHandle = IntPtr.Zero;

        public MainWindow()
        {
            InitializeComponent();

            // フックの設定
            var hWnd = new WindowInteropHelper(Application.Current.MainWindow).EnsureHandle();
            HwndSource source = HwndSource.FromHwnd(hWnd);
            source.AddHook(new HwndSourceHook(WndProc));

            // WM_POWERBROADCAST > PBT_POWERSETTINGCHANGE > GUID_CONSOLE_DISPLAY_STATE が取れるように登録
            registerConsoleDisplayHandle = RegisterPowerSettingNotification(hWnd, ref GUID_CONSOLE_DISPLAY_STATE, DEVICE_NOTIFY_WINDOW_HANDLE);
            registerConsoleDisplayHandle = RegisterPowerSettingNotification(hWnd, ref GUID_BATTERY_PERCENTAGE_REMAINING, DEVICE_NOTIFY_WINDOW_HANDLE);
            registerConsoleDisplayHandle = RegisterPowerSettingNotification(hWnd, ref GUID_MONITOR_POWER_ON, DEVICE_NOTIFY_WINDOW_HANDLE);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (registerConsoleDisplayHandle != IntPtr.Zero)
                UnregisterPowerSettingNotification(registerConsoleDisplayHandle);
        }

        // メッセージループを記述したメソッド
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_POWERBROADCAST)
            {
                switch (wParam.ToInt32())
                {
                    case PBT_POWERSETTINGCHANGE:
                        var pbs = (POWERBROADCAST_SETTING)Marshal.PtrToStructure(lParam, typeof(POWERBROADCAST_SETTING));
                        if (pbs.PowerSetting == GUID_CONSOLE_DISPLAY_STATE)
                        {
                            Debug.WriteLine("GUID_CONSOLE_DISPLAY_STATE：" + pbs.Data.ToString());
                        }
                        if (pbs.PowerSetting == GUID_BATTERY_PERCENTAGE_REMAINING)
                        {
                            Debug.WriteLine("GUID_BATTERY_PERCENTAGE_REMAINING：" + pbs.Data.ToString());
                        }
                        if (pbs.PowerSetting == GUID_MONITOR_POWER_ON)
                        {
                            Debug.WriteLine("GUID_MONITOR_POWER_ON：" + pbs.Data.ToString());
                        }
                        break;
                }
            }
            return IntPtr.Zero;
        }
    }
}
