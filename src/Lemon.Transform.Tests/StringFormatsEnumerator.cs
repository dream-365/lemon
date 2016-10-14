using System;
using System.Collections;
using System.Collections.Generic;

namespace Lemon.Transform.Tests
{
    public class StringFormatsEnumerator : IEnumerator<string>
    {
        private string _format;

        private long _start;

        private long _end;

        private long _currentIndex;

        public StringFormatsEnumerator(string format, long start, long end)
        {
            _format = format;

            _start = start;

            _end = end;

            _currentIndex = start - 1;
        }

        public string Current
        {
            get
            {
                return string.Format(_format, _currentIndex);
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

        }

        public bool MoveNext()
        {
            _currentIndex++;

            Console.Write("[{0}] ", _currentIndex);

            return !(_currentIndex > _end);
        }

        public void Reset()
        {
            _currentIndex = _start - 1;
        }
    }
}
