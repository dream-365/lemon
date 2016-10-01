using Lemon.Transform;

namespace Demo001
{
    public class OutOfMemoryDataPipeline : DataFlowPipeline
    {
        protected override AbstractDataInput OnCreate(PipelineContext context)
        {
            var input = new RandomDataInput(10) { Context = context };

            var slowAction = new SlowAction { Context = context };

            var debug = new DebugOutput { Context = context };

            var dummy = new DummyOutput { Context = context };

            input.Link.BroadCast(slowAction, dummy);

            slowAction.Link.SuccessTo(debug).End();

            Waits(debug, dummy);

            return input;
        }
    }
}
