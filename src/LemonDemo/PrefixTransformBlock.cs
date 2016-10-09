using Lemon.Data.Core;
using System;

namespace LemonDemo
{
    public class PrefixTransformBlock : ITransformBlock<int, string>
    {
        private string _prefix;

        public PrefixTransformBlock(string prefix)
        {
            _prefix = prefix;
        }

        public string Transform(int record)
        {
            if (record % 5 == 0)
            {
                throw new Exception("ex");
            }

            return _prefix + (record + 100000).ToString();
        }
    }
}
