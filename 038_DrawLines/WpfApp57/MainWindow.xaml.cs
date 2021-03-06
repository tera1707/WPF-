﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp57
{
    /// <summary>
    /// ボタンをおしたら、サインカーブを書く
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // ------------------------------------------------

        // サインカーブの縦方向の真ん中座標
        private double y = 150.0;

        public List<Point> Points
        {
            get { return _points; }
            set { _points = value; OnPropertyChanged(nameof(Points)); }
        }
        private List<Point> _points = new List<Point>();
        
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
            Debug.WriteLine(e.Mode);

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
            Debug.WriteLine(e.Reason);

            switch (e.Reason)
            {
                case SessionSwitchReason.ConsoleConnect:
                    Debug.WriteLine("ConsoleConnect");
                    break;
                case SessionSwitchReason.ConsoleDisconnect:
                    Debug.WriteLine("ConsoleDisconnect");
                    break;
                case SessionSwitchReason.RemoteConnect:
                    Debug.WriteLine("RemoteConnect");
                    break;
                case SessionSwitchReason.RemoteDisconnect:
                    Debug.WriteLine("RemoteDisconnect");
                    break;
                default:
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var _ = Task.Run(() =>
            {
                this.Dispatcher.Invoke(new Action(async () =>
                {
                    for (int i = 0; i < 100; i++)
                    {
                        Points.Add(new Point((double)i, y + 150.0 * Math.Sin((double)i / (Math.PI))));
                        OnPropertyChanged(nameof(Points));
                        await Task.Delay(30);
                    }
                }));
            });

        }
    }
}
