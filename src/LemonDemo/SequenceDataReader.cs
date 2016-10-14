using Lemon.Data.Core;
using System;
using System.Collections;

namespace LemonDemo
{
    public class SequenceDataReader : IDataReader<int>
    {
        private int _index = 0;

        private int _max = int.MaxValue;

        private int _current;

        public SequenceDataReader(int max)
        {
            _max = max;
        }

        public void Dispose()
        {

        }

        public bool Next()
        {
            _index++;

            if (_index > _max)
            {
                return false;
            }

            _current = _index;

            return true;
        }

        public int Read()
        {
           return _current;
        }

        object IDataReader.Read()
        {
            return Read();
        }
    }
}
