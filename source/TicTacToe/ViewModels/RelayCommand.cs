using System;
using System.Diagnostics;
using System.Windows.Input;

namespace TicTacToe.ViewModels
{
    [DebuggerNonUserCode]
    public class RelayCommand : ICommand
    {
        private readonly Action<object>     _execute;
        private readonly Func<object, bool> _canExecute;
        //---------------------------------------------------------------------
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute    = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
        //---------------------------------------------------------------------
        public void Execute(object parameter)    => _execute(parameter);
        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;
        //---------------------------------------------------------------------
        public event EventHandler CanExecuteChanged
        {
            add    { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
