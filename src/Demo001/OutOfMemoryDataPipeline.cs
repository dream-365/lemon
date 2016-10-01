using Lemon.Transform;

namespace Demo001
{
    public class OutOfMemoryDataPipeline : DataFlowPipeline
    {
        protected override AbstractDataInput OnCreate(PipelineContext context)
        {
            var input = new InfiniteDataInput { Context = context };

            var slowAction = new SlowAction { Context = context };

            var debug = new DebugOutput { Context = context };

            input.LinkTo(slowAction);

            slowAction.Link.SuccessTo(debug);

            Waits(debug);

            return input;
        }
    }
}
