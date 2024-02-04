using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KaddaOK.Library
{
    public abstract class ObservableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void RaisePropertyChanged(string? propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Set a property and raise a property changed event if it has changed
        /// </summary>
        protected bool SetProperty<T>(ref T property, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(property, value))
            {
                return false;
            }

            property = value;
            RaisePropertyChanged(propertyName);
            return true;
        }
    }
}
