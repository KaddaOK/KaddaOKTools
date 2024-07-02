using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace KaddaOK.AvaloniaApp
{
    public class ObservableStack<T> : ObservableCollection<T>
    {
        public T? Peek => Items.LastOrDefault();

        public T? Pop()
        {
            var popIndex = Items.Count - 1;
            var itemToPop = Items[popIndex];
            if (itemToPop != null)
            {
                RemoveAt(popIndex);
            }

            return itemToPop;
        }

        public void Push(T? item)
        {
            Add(item);
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Peek)));
        }
    }
}
