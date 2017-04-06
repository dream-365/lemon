using System.Collections.Generic;

namespace Lemon.Core
{
    public class DataReaderWrapper<T> : IDataReader<T>
    {
        private readonly IEnumerator<T> _enumerator;

        public DataReaderWrapper(IEnumerator<T> enumerator)
        {
            _enumerator = enumerator;
        }

        public DataReaderWrapper(IEnumerable<T> enumerable) : 
            this(enumerable.GetEnumerator())
        {
        }

        public void Dispose()
        {
            _enumerator.Dispose();
        }

        public bool Next()
        {
            return _enumerator.MoveNext();
        }

        public T Read()
        {
            return _enumerator.Current;
        }

        object IDataReader.Read()
        {
            return Read();
        }
    }
}
