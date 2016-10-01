using Lemon.Transform;

namespace Demo001
{
    public class OutOfMemoryDataPipeline : DataFlowPipeline
    {
        protected override AbstractDataInput OnCreate(PipelineContext context)
        {
            var input = new RandomDataInput(300000) { Context = context };

            var debug = new DebugOutput { Context = context };

            var slowAction = context.Attach(new SlowAction());

            input.Link.SuccessTo(slowAction).End();

            slowAction.Link.SuccessTo(debug).End();

            Waits(debug);

            return input;
        }
    }
}
