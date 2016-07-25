using Lemon.Transform;

namespace Demo001
{
    public class MongoToDebugDemo : DataFlowPipeline
    {
        protected override AbstractDataInput OnCreate(PipelineContext context)
        {
            var input = context.IO.GetInput("landing_questions");

            var debug = new DebugOutput();

            input.LinkTo(debug);

            Waits(debug);

            return input;
        }
    }
}
