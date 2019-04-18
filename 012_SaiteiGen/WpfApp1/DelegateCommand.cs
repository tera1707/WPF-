using System.Windows.Input;

namespace WpfApp1
{
    public class DelegateCommand : ICommand
    {
        System.Action execute;
        System.Func<bool> canExecute;

        public bool CanExecute(object parameter)
        {
            return canExecute();
        }

        public event System.EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            execute();
        }

        public DelegateCommand(System.Action execute)
        {
            this.execute = execute;
        }

        public DelegateCommand(System.Action execute, System.Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }
    }
}
