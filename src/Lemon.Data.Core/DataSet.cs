using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lemon.Data.Core
{
    public class DataSet<T> : IEnumerable<T>
    {
        private const int DEFAULT_LENGTH_OF_PAGE = 1000;

        private int _lengthOfPage;

        public DataSet(IDataReader<T> source)
        {
            _lengthOfPage = DEFAULT_LENGTH_OF_PAGE;
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
