using System.Collections.Generic;
using Lemon.Data.Core;

namespace Lemo.Data.Core.Tests
{
    public class Pair<T>
    {
        public T Previous { get; set; }
        public T Current { get; set; }
    }

    public class AssertObserver<T> : ICompareObserver<T>
    {
        private IList<T> _addList;
        private IList<T> _deleteList;
        private IList<Pair<T>> _changeList;

        public IEnumerable<T> Adds { get { return _addList; } }
        public IEnumerable<T> Deletes { get { return _deleteList;  } }
        public IEnumerable<Pair<T>> Changes { get { return _changeList; } }

        public AssertObserver()
        {
            _addList = new List<T>();
            _deleteList = new List<T>();
            _changeList = new List<Pair<T>>();
        }

        public void OnAdd(T message)
        {
            _addList.Add(message);
        }

        public void OnChange(T previous, T current)
        {
            _changeList.Add(new Pair<T> { Previous = previous, Current = current });
        }

        public void OnDelete(T message)
        {
            _deleteList.Add(message);
        }
    }
}
