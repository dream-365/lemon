using Lemon.Transform;

namespace Demo001
{
    public class UpdateOnChangeDemo : DataFlowPipeline
    {
        protected override AbstractDataInput OnCreate(PipelineContext context)
        {
            var input = context.IO.GetInput("test_data");

            var output = context.IO.GetOutput("sql_tbl001");

            var statusContext = output.GetDataRowStatusContext(new string[] { });

            var compare = context.Attach(new CompareStatusAction(statusContext));

            var broadcast = new BroadcastAction();

            var debug = new DebugOutput();

            input.LinkTo(compare);

            compare.Link.SuccessTo(broadcast, row => row != null).End();

            broadcast.Link.SuccessTo(debug).End();

            broadcast.Link.SuccessTo(output).End();

            EnsureComplete(debug.Compltetion);

            return input;
        }
    }
}
