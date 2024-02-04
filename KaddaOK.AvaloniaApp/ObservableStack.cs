using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace KaddaOK.AvaloniaApp
{
    /// <summary>
    /// Per https://stackoverflow.com/a/56177896/
    /// </summary>
    public class ObservableStack<T> : Stack<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        #region Constructors

        public ObservableStack() : base() { }

        public ObservableStack(IEnumerable<T> collection) : base(collection) { }

        public ObservableStack(int capacity) : base(capacity) { }

        #endregion

        #region Overrides

        public new virtual T? Pop()
        {
            var item = base.Pop();
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, item);

            return item;
        }

        public new virtual void Push(T? item)
        {
            if (item != null)
            {
                base.Push(item);
                OnCollectionChanged(NotifyCollectionChangedAction.Add, item);
            }
        }

        public new virtual void Clear()
        {
            base.Clear();
            OnCollectionChanged(NotifyCollectionChangedAction.Reset, default);
        }

        #endregion

        #region CollectionChanged

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        protected virtual void OnCollectionChanged(NotifyCollectionChangedAction action, T? item)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(
                action
                , item
                , item == null ? -1 : 0)
            );

            OnPropertyChanged(nameof(Count));
        }

        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string proertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(proertyName));
        }

        #endregion
    }
}
