using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Transform.Tests
{
    public class FakeDataStoreService : IDataStoreService
    {
        public DataInputModel GetDataInput(string name)
        {
            return new DataInputModel
            {
                SourceType = "fakedb",
                ObjectName = "landing.office_365_threads",
                Connection = "mongodb://localhost:27017",
                Filter = "{}"
            };
        }

        public DataOutputModel GetDataOutput(string name)
        {
            return new DataOutputModel
            {
                TargetType = "fakedb",
                Connection = "mongodb://localhost:27017",
                ObjectName = "landing.output_test",
                ColumnNames = new string[] { "Id", "Title", "CreatedOn", "CreateBy"},
                IsUpsert = true,
                PrimaryKey = "Id"
            };
        }
    }
}
