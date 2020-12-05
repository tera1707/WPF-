using Microsoft.Win32;
using System;
using System.Windows;

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

            tbTargetKey.Text = @"SOFTWARE\Classes\CLSID\{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\LocalServer32";
            tbValue.Text = System.Reflection.Assembly.GetExecutingAssembly().Location;
        }

        private void make_Click(object sender, RoutedEventArgs e)
        {
            using (var key = Registry.CurrentUser.OpenSubKey(tbTargetKey.Text))
            {
                if (string.Equals(key?.GetValue(null) as string, tbValue.Text, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("すでに同じ値が登録済みです");
                    return;
                }
            }
            using (var key = Registry.CurrentUser.CreateSubKey(tbTargetKey.Text))
            {
                key.SetValue(null, tbValue.Text);
            }
        }

        private void read_Click(object sender, RoutedEventArgs e)
        {
            using (var key = Registry.CurrentUser.OpenSubKey(tbTargetKey.Text))
            {
                if (key == null)
                {
                    MessageBox.Show("キーがありません");
                }
                else
                {
                    tbValue.Text = (string)key.GetValue(null);
                }
            }
        }
    }
}
