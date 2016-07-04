using System.Linq;
using System.Collections.Generic;

namespace Lemon.Transform
{
    public class ProgressIndicator
    {
        private IDictionary<string, long> _dictionary = new Dictionary<string, long>();

        public long Increment(string key)
        {
            if (!_dictionary.ContainsKey(key))
            {
                lock (_dictionary)
                {
                    _dictionary.Add(key, 0);
                }
            }

            var count = _dictionary[key] + 1;

            _dictionary[key] = count;

            return count;
        }

        public  IEnumerable<KeyValuePair<string, long>> GetAllProgress()
        {
            return _dictionary.ToList();
        }

        public void Clear()
        {
            _dictionary.Clear();
        }
    }
}
