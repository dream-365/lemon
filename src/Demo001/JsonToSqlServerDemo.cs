using Lemon.Transform;

namespace Demo001
{
    public class JsonToSqlServerDemo : DataFlowPipeline
    {
        protected override AbstractDataInput OnCreate(PipelineContext context)
        {
            var input = new JsonFileDataInput("test_data.json");

            var output = context.IO.GetOutput("sql_data_output");

            input.Link.SuccessTo(output).End();

            Waits(output);

            return input;
        }
    }
}
