using Lemon.Transform;
using Lemon.Transform.Models;
using System.Collections.Generic;
using System;

namespace LemonDemo
{
    public class PrefixTransformBlock : ITransformBlock<IDictionary<string, object>, IDictionary<string, object>>
    {
        private string _prefix;

        public PrefixTransformBlock(string prefix)
        {
            _prefix = prefix;
        }

        public IDictionary<string, object> Transform(IDictionary<string, object> record)
        {
            record["prefix"] = _prefix;

            return record;
        }
    }
}
