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

        public override void Start(IDictionary<string, object> parameters = null)
        {
            foreach(var row in _rows)
            {
                Post(row);
            }

            Complete();
        }
    }
}
