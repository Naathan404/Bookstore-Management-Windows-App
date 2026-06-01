using System;
using System.Windows.Input;

namespace Bookstore.WPF.Services
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Predicate<T> _canExecute;
        private readonly Action<T> _execute;

        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        private EventHandler? _canExecuteChanged;
        public event EventHandler? CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                _canExecuteChanged += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
                _canExecuteChanged -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return parameter is T t && (_canExecute?.Invoke(t) ?? true);
        }

        public void Execute(object parameter)
        {
            if (parameter is T t)
                _execute(t);
        }

        public void RaiseCanExecuteChanged()
        {
            _canExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}