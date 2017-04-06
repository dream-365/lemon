using System.Collections.Generic;

namespace Lemon.Core
{
    public class ListWriter<T> : IDataWriter<T>
    {
        private readonly IList<T> _list;
        public ListWriter(IList<T> list)
        {
            _list = list;
        }
        public void Dispose()
        {
        }

        public void Write(IEnumerable<T> records)
        {
            foreach (var record in records)
                _list.Add(record);
        }

        public void Write(T record)
        {
            _list.Add(record);
        }
    }
}
