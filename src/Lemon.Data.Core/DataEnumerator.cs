using System;
using System.Collections;
using System.Collections.Generic;

namespace Lemon.Data.Core
{
    public class DataEnumerator<T> : IEnumerator<T>
    {
        private IDataReader<T> _reader;

        public DataEnumerator(IDataReader<T> reader)
        {
            _reader = reader;
        }

        public T Current
        {
            get
            {
                return _reader.Read();
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public void Dispose()
        {
            _reader.Dispose();
        }

        public bool MoveNext()
        {
            return _reader.Next();
        }

        public void Reset()
        {
            
        }
    }
}
