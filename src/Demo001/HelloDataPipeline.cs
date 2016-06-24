using Lemon.Transform;

namespace Demo001
{
    public class HelloDataPipeline : DataFlowPipeline
    {
        protected override AbstractDataInput OnCreate(PipelineContext context)
        {
            var input = context.IO.GetInput("landing_threads");

            var output1 = context.IO.GetOutput("console_user_activity");

            var output2 = context.IO.GetOutput("user_activity");

            var action1 = new UserActiviesTransformAction();

            var broadcast = new BroadcastAction();

            input.LinkTo(action1);

            action1.LinkTo(broadcast);

            broadcast.LinkTo(output1);

            broadcast.LinkTo(output2);

            EnsureComplete(output1.Compltetion);

            EnsureComplete(output2.Compltetion);

            return input;
        }
    }
}
