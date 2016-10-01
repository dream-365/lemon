using Lemon.Transform;
using System;

namespace Demo001
{
    public class WriteOnChangeDemo : DataFlowPipeline
    {
        protected override AbstractDataInput OnCreate(PipelineContext context)
        {
            var input = context.Attach(new JsonFileDataInput("test_data.json"));

            var output = context.IO.GetOutput("sql_data_output");

            output.AfterWrite = (row) =>
            {
                Console.WriteLine(row.ToString());
            };

            input.Link.SuccessTo(output).End();

            Waits(output);

            return input;
        }
    }
}
