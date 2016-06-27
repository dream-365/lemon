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

            action1.Link.SuccessTo(broadcast).End();

            broadcast.Link.SuccessTo(output1).End();

            broadcast.Link.SuccessTo(output2).End();

            EnsureComplete(output1.Compltetion);

            EnsureComplete(output2.Compltetion);

            return input;
        }
    }
}
