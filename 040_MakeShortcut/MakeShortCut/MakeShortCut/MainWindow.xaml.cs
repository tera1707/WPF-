using DesktopToast.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MakeShortCut
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            tbShortcutPath.Text = @"C:\Users\masa\AppData\Roaming\Microsoft\Windows\Start Menu\MyShortcut.lnk";
            tbTargetPath.Text = System.Reflection.Assembly.GetExecutingAssembly().Location;
        }

        private void make_Click(object sender, RoutedEventArgs e)
        {
            var shortcut = new Shortcut();

            shortcut.InstallShortcut(
                tbShortcutPath.Text,
                tbTargetPath.Text,
                "",
                "",
                System.IO.Path.GetDirectoryName(tbTargetPath.Text),
                Shortcut.ShortcutWindowState.Normal,
                "",
                tbAumid.Text,
                new Guid(tbClsid.Text)
                );
        }

        private void read_Click(object sender, RoutedEventArgs e)
        {
            var shortcut = Shortcut.ReadShortcut(tbShortcutPath.Text);

            tbAumid.Text = shortcut.AumId;
            tbClsid.Text = shortcut.activatorId.ToString();

        }
    }
}
