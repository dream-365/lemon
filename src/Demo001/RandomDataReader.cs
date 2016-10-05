using Lemon.Transform;
using System.Collections.Generic;
using System;

namespace LemonDemo
{
    public class RandomDataReader : IDataReader<IDictionary<string, object>>
    {
        private bool _end = false;

        private long _index = 0;

        private long _max = int.MaxValue;

        public RandomDataReader(int max)
        {
            _max = max;
        }

        public IDictionary<string, object> Read()
        {
            var row = new Dictionary<string, object>
            {
                {"id", _index }
            };

            _end = _index > _max;

            return row;
        }

        bool IDataReader<IDictionary<string, object>>.End()
        {
            return _end;
        }
    }
}
