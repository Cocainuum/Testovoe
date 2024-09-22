using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientApp.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Func<object, Task> _execute;
        private readonly Func<object, bool> _canExecute;
 
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
 
        public RelayCommand(Func<object, Task> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
 
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }
 
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}