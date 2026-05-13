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

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            if (_canExecute == null) return true;

            // Nếu parameter là null và T là kiểu tham chiếu (Reference Type), cứ cho nó chạy
            if (parameter == null)
            {
                return _canExecute(default(T)!);
            }

            return _canExecute((T)parameter);
        }

        public void Execute(object? parameter)
        {
            // Xử lý an toàn tương tự cho hàm Execute
            if (parameter == null)
            {
                _execute(default(T)!);
            }
            else
            {
                _execute((T)parameter);
            }
        }

        //public bool CanExecute(object? parameter)
        //{
        //    return _canExecute == null || _canExecute((T)parameter!);
        //}

        //public void Execute(object? parameter)
        //{
        //    _execute((T)parameter!);
        //}

        //public event EventHandler? CanExecuteChanged
        //{
        //    add { CommandManager.RequerySuggested += value; }
        //    remove { CommandManager.RequerySuggested -= value; }
        //}

        //public void RaiseCanExecuteChanged()
        //{
        //    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        //}
    }
}