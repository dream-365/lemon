using System.Collections.Generic;

namespace Lemon.Core
{
    public class ComparingPipe<T>
    {
        private readonly CompareOptions _options;
        private readonly IComparer<T> _primaryKeyComparer;
        private readonly IEqualityComparer<T> _fieldsEqualityComparer;
        private readonly ICompareObserver<T> _observer;

        public ComparingPipe(CompareOptions options, ICompareObserver<T> observer)
        {        
            _primaryKeyComparer = ServicesInstaller.Current.Container
                .Resolve<IComparer<T>>(new { primaryKey = options.PrimaryKey });
            _fieldsEqualityComparer = ServicesInstaller.Current.Container
                .Resolve<IEqualityComparer<T>>(new { fieldsToCompare = options.ColumnsToCompare });
            _observer = observer;
            _options = options;
        }

        public void Compare(DataSet<T> set, DataSet<T> with)
        {
            Compare(set.GetEnumerator(), with.GetEnumerator());
        }

        public void Compare(IEnumerable<T> set, IEnumerable<T> with)
        {
            Compare(set.GetEnumerator(), with.GetEnumerator());
        }

        public void Compare(IEnumerator<T> set, IEnumerator<T> with)
        {
            bool endOfSet = false;
            bool endOfWith = false;

            endOfSet = !set.MoveNext();
            endOfWith = !with.MoveNext();

            do
            {
                if (endOfSet)
                {
                    _observer.OnAdd(with.Current);
                    endOfWith = !with.MoveNext();
                    continue;
                }

                if (endOfWith)
                {
                    _observer.OnDelete(set.Current);
                    endOfSet = !set.MoveNext();
                    continue;
                }

                var compareResult = _primaryKeyComparer.Compare(set.Current, with.Current);

                if (compareResult > 0)
                {
                    _observer.OnAdd(with.Current);
                    endOfWith = !with.MoveNext();
                }
                else if (compareResult < 0)
                {
                    _observer.OnDelete(set.Current);
                    endOfSet = !set.MoveNext();
                }
                else
                {
                    if (!_fieldsEqualityComparer.Equals(set.Current, with.Current))
                    {
                        _observer.OnChange(set.Current, with.Current);
                    }

                    endOfSet = !set.MoveNext();
                    endOfWith = !with.MoveNext();
                }

            } while (!endOfSet || !endOfWith);
        }
    }
}
