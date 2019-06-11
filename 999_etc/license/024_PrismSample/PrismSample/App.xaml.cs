﻿using System.Windows;

namespace PrismSample
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Bootstrapperを起動する
            new Bootstrapper().Run();
        }
    }
}
