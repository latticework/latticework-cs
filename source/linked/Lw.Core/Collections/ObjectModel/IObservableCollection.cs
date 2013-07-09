using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Lw.Collections.ObjectModel
{
    public interface IObservableCollection 
        : IList, ICollection, IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged
    {
    }

    public interface IObservableCollection<T>
        : IObservableCollection, 
        IList<T>, 
        ICollection<T>, 
        IEnumerable<T>, 
        INotifyCollectionChanged, 
        INotifyPropertyChanged
    {
    }
}
