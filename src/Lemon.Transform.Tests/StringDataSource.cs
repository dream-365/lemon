using System.Collections;
using System.Collections.Generic;

namespace Lemon.Transform.Tests
{
    public class StringDataSource : IEnumerable<string>
    {
        private IEnumerator<string> _enumerator;

        public StringDataSource(string formats, long begin, long end)
        {
            _enumerator = new StringFormatsEnumerator(formats, begin, end);
        }


        public IEnumerator<string> GetEnumerator()
        {
            return _enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
