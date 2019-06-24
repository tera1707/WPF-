using System.Windows;
using System.Windows.Input;

namespace WpfApp1
{
    class ViewModel
    {
        public ICommand func { get; set; }

        public ViewModel()
        {
            func = new DelegateCommand(
                () =>
                {
                    MessageBox.Show("トリガー発火");
                    return;
                });
        }

        public void EventFunc()
        {
            MessageBox.Show("CallMethodAction でトリガー発火(ViewModelのメソッド)");
        }
    }
}
