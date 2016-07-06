using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lemon.Transform;

namespace Demo001
{
    public class DataPipelineDemo4 : DataFlowPipeline
    {
        protected override AbstractDataInput OnCreate(PipelineContext context)
        {
            var list = new List<BsonDataRow>();

            list.Add(new BsonDataRow(new MongoDB.Bson.BsonDocument { { "Id", Guid.NewGuid().ToString() }, { "Timestamp", DateTime.Now }}));

            var input = new ListDataInput(list);

            var output = context.IO.GetOutput("sql_tbl002");

            input.LinkTo(output);

            EnsureComplete(output.Compltetion);

            return input;
        }
    }
}
