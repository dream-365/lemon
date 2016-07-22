using Lemon.Transform;
using System;

namespace Demo001
{
    public class WriteOnChangeDemo : DataFlowPipeline
    {
        protected override AbstractDataInput OnCreate(PipelineContext context)
        {
            var input = context.IO.GetInput("test_data");

            var output = context.IO.GetOutput("sql_tbl001");

            output.AfterWrite = (row) =>
            {
                Console.WriteLine(row.ToString());
            };

            input.LinkTo(output);

            Waits(output);

            return input;
        }
    }
}
