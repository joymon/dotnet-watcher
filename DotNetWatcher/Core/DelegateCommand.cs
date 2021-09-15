using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace JoymonsCode.DotNetWatcher.Core
{
    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;

        public DelegateCommand(Action<T> execute)
            : this(execute, x => true)
        {
        }

        public DelegateCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (canExecute == null) throw new ArgumentNullException("canExecute");
            if (execute == null) throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }
        #region "ICommand"
        
        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute((T)parameter);
        }

        public event EventHandler CanExecuteChanged;

        #endregion
    }

    public class DelegateCommand : DelegateCommand<object>
    {
        public DelegateCommand(Action execute)
            : base(execute != null ? x => execute() : (Action<object>)null)
        {
        }

        public DelegateCommand(Action execute, Func<bool> canExecute)
            : base(execute != null ? x => execute() : (Action<object>)null,
                    canExecute != null ? x => canExecute() : (Predicate<object>)null)
        {
        }
    }
}
