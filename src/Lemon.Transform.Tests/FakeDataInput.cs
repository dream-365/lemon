using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Transform.Tests
{
    public class FakeDataInput : IDataInput
    {
        private Action<BsonDataRow> _outputFunction;


        public FakeDataInput(DataInputModel model)
        {

        }

        public Action<BsonDataRow> Output
        {
            set
            {
                _outputFunction = value;
            }
        }

        public void Start()
        {
            var row = new BsonDataRow();

            row.SetValue("_id", "id_value");

            row.SetValue("title", "_title_");

            row.SetValue("createdOn", DateTime.Now);

            row.SetValue("createBy", "_create_by_");

            _outputFunction(row);
        }
    }
}
