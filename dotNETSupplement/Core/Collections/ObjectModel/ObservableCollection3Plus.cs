using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ch3plusStudio.dotNETSupplement.Core.Collections.ObjectModel
{
    public class ObservableCollection3Plus<T> : ObservableCollection<T>
    {
        public ObservableCollection3Plus() : base() { }
        public ObservableCollection3Plus(IEnumerable<T> collection) : base(collection) { }
        public ObservableCollection3Plus(List<T> list) : base(list) { }

        public void BlockOps(Action<IList<T>> action)
        {
            action(Items);

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        }
    }
}
