using System;
using System.Collections.Generic;

namespace Lemon.Transform
{
    public abstract class TransformAction
    {
        public string Name { get; protected set; }

        public Action<BsonDataRow> Output;

        public IEnumerable<string> InputColumnNames { get; set; }

        public IEnumerable<string> OutputColumnNames { get; set; }

        public abstract void Input(BsonDataRow row);
    }
}
