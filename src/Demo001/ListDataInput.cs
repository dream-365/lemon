using System.Collections.Generic;
using Lemon.Transform;

namespace Demo001
{
    public class ListDataInput : AbstractDataInput
    {
        private IEnumerable<BsonDataRow> _rows;

        public ListDataInput(IEnumerable<BsonDataRow> rows)
        {
            _rows = rows;
        }

        public override void Start()
        {
            foreach(var row in _rows)
            {
                Post(row);
            }

            Complete();
        }
    }
}
