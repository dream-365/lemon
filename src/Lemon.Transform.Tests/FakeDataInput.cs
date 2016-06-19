using System;

namespace Lemon.Transform.Tests
{
    public class FakeDataInput : AbstractDataInput
    {
        public FakeDataInput(DataInputModel model)
        {

        }

        public override void Start()
        {
            var row = new BsonDataRow();

            row.SetValue("_id", "id_value");

            row.SetValue("title", "_title_");

            row.SetValue("createdOn", DateTime.Now);

            row.SetValue("createBy", "_create_by_");

            Post(row);
        }
    }
}
