using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using CommunityToolkit.Mvvm.Input;

namespace KaddaOK.AvaloniaApp
{
    public interface IReportReasonCantExecute
    {
        string ReasonCantExecute { set; }
    }

    public class RelayCommandWithReason :
        IRelayCommand /* from CommunityToolkit.MVVM */,
        IReportReasonCantExecute,
        INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region INotifyPropertyChange interfaces

        public event PropertyChangedEventHandler? PropertyChanged;
        public event PropertyChangingEventHandler? PropertyChanging;

        #endregion

        #region IRelayCommand interface

        public event EventHandler? CanExecuteChanged;

        public void NotifyCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Execute(object? parameter)
        {
            _execute?.Invoke(parameter);
        }

        public bool CanExecute(object? parameter)
        {
            if (_canExecuteWithReason != null)
            {
                bool canExecute = _canExecuteWithReason(parameter, this);

                if (canExecute)
                {
                    ReasonCantExecute = null;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReasonCantExecute)));
                return canExecute;
            }

            return true;
        }

        #endregion

        public delegate bool CanExecuteWithReason(object? parameter, IReportReasonCantExecute reporter);

        private string? _reasonCantExecute;
        public string? ReasonCantExecute
        {
            get => _reasonCantExecute;
            set
            {
                if (_reasonCantExecute != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(ReasonCantExecute)));
                    _reasonCantExecute = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReasonCantExecute)));
                }
            }
        }

        private readonly Action<object?>? _execute;
        private readonly CanExecuteWithReason? _canExecuteWithReason;

        public RelayCommandWithReason(Action<object?> execute, CanExecuteWithReason canExecuteWithReason)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecuteWithReason = canExecuteWithReason;
        }
    }
}
