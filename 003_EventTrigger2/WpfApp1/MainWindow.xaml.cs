using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new ViewModel();
        }

        public void CodeBehindFunc()
        {
            MessageBox.Show("CallMethodAction でトリガー発火(コードビハインドのメソッド)");
        }
    }
}
