using Lemon.Transform;
using Lemon.Transform.Models;
using System.Collections.Generic;
using System;

namespace LemonDemo
{
    public class PrefixTransformBlock : ITransformBlock<int, int>
    {
        private string _prefix;

        public PrefixTransformBlock(string prefix)
        {
            _prefix = prefix;
        }

        public int Transform(int record)
        {
            return record + 100000;
        }
    }
}
