using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace KaddaOK.AvaloniaApp
{
    /// <summary>
    /// Per https://stackoverflow.com/a/56177896/
    /// </summary>
    public class ObservableQueue<T> : Queue<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        #region Constructors

        public ObservableQueue() : base() { }

        public ObservableQueue(IEnumerable<T> collection) : base(collection) { }

        public ObservableQueue(int capacity) : base(capacity) { }

        #endregion

        #region Overrides

        public new virtual T? Dequeue()
        {
            var item = base.Dequeue();
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, item);

            return item;
        }

        public new virtual bool TryDequeue(out T? result)
        {

            var returnValue = base.TryDequeue(out result);
            if (returnValue)
            {
                OnCollectionChanged(NotifyCollectionChangedAction.Remove, result);
            }

            return returnValue;
        }

        public new virtual void Enqueue(T? item)
        {
            if (item != null)
            {
                base.Enqueue(item);
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
