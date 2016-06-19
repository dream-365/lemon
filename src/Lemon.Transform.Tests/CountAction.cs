using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Transform.Tests
{
    public class CountAction : TransformSingleAction
    {
        private int _count = 0;

        public override BsonDataRow Transform(BsonDataRow row)
        {
            _count++;

            return row;
        }

        public int Count { get { return _count; } }
    }
}
