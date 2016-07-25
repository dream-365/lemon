using Lemon.Transform;

namespace Demo001
{
    public class SqlQueryToDebugDemo : DataFlowPipeline
    {
        protected override AbstractDataInput OnCreate(PipelineContext context)
        {
            var input = context.IO.GetInput("msdn_user_ranks");

            var output = new DebugOutput();

            input.LinkTo(output);

            Waits(output);

            return input;
        }
    }
}
