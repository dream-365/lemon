using Lemon.Transform;
using System;

namespace LemonDemo
{
    public class RandomDataReader : IDataReader<int>
    {
        private bool _end = false;

        private long _index = 0;

        private long _max = int.MaxValue;

        public RandomDataReader(int max)
        {
            _max = max;
        }

        public void Close()
        {
            
        }

        public bool End()
        {
            return _end;
        }

        public int Read()
        {
            _index++;

            if(_index > _max)
            {
                _end = true;
            }

            return (int)_index;
        }

        public object ReadObject()
        {
            return Read();
        }
    }
}
