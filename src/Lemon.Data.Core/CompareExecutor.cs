using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lemon.Data.Core
{
    [Obsolete]
    public class CompareExecutor<T> : IComareExecute<T>
    {
        private IEnumerator<T>  _set;

        private IEnumerator<T> _with;

        private CompareOptions _options;

        public CompareExecutor(DataSet<T> set, DataSet<T> with, CompareOptions options)
        {
            _set = set.GetEnumerator();

            _with = with.GetEnumerator();

            _options = options;
        }

        private ICompareObserver<T> _observer;

        public ICompareObserver<T> Observer
        {
            get
            {
                return _observer;
            }

            set
            {
                _observer = value;
            }
        }

        private void Compare()
        {
            bool endOfSet = false;

            bool endOfWith = false;

            var primaryKeyComparer = new PrimaryKeyComparer<T>(_options.PrimaryKey);

            var equalityComparer = new FieldsEqualityComparer<T>(_options.ColumnsToCompare);

            endOfSet = !_set.MoveNext();
            endOfWith = !_with.MoveNext();

            do
            {
                if (endOfSet)
                {
                    _observer.OnAdd(_with.Current);

                    endOfWith = !_with.MoveNext();

                    continue;
                }

                if(endOfWith)
                {
                    _observer.OnDelete(_set.Current);

                    endOfSet = !_set.MoveNext();

                    continue;
                }

                var compareResult = primaryKeyComparer.Compare(_set.Current, _with.Current);

                if(compareResult > 0)
                {
                    _observer.OnAdd(_with.Current);
                    endOfWith = !_with.MoveNext();
                } else if (compareResult < 0)
                {
                    _observer.OnDelete(_set.Current);
                    endOfSet = !_set.MoveNext();
                } else
                {
                    if(!equalityComparer.Equals(_set.Current, _with.Current))
                    {
                        _observer.OnChange(_set.Current, _with.Current);
                    }

                    endOfSet = !_set.MoveNext();
                    endOfWith = !_with.MoveNext();
                }

            } while (!endOfSet||!endOfWith);
        }

        public Task RunAsync()
        {
            return Task.Run(() =>
            {
                Compare();
            });
        }

        public void Run()
        {
            Compare();
        }
    }
}
