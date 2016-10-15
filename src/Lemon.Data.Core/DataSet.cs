using System.Collections;
using System.Collections.Generic;

namespace Lemon.Data.Core
{
    public class DataSet<T> : IEnumerable<T>
    {
        private const int DEFAULT_LENGTH_OF_PAGE = 1000;

        private int _lengthOfPage;

        private IEnumerator<T> _enumerator;

        public DataSet(IEnumerable<T> source)
        {
            _lengthOfPage = DEFAULT_LENGTH_OF_PAGE;

            _enumerator = new PageEnumerator<T>(source.GetEnumerator(), _lengthOfPage);
        }

        public DataSet(IDataReader<T> source)
        {
            _lengthOfPage = DEFAULT_LENGTH_OF_PAGE;

            _enumerator = new PageEnumerator<T>(source.AsEnumerator(), _lengthOfPage);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
