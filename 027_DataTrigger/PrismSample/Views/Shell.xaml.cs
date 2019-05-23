using Microsoft.Practices.Unity;
using Prism.Regions;
using System;
using System.Windows;

namespace PrismSample.Views
{
    /// <summary>
    /// Shell.xaml の相互作用ロジック
    /// </summary>
    public partial class Shell : Window
    {
        [Dependency]
        public IRegionManager RegionManager { get; set; }
        public Shell()
        {
            InitializeComponent();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            this.RegionManager.RequestNavigate("MainRegion", nameof(UserControl1));
        }
    }
}
