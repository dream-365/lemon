using System;
using Lemon.Transform;

namespace Demo001
{
    public class DatapipelineDemo2 : DataFlowPipeline
    {
        protected override AbstractDataInput OnCreate(PipelineContext context)
        {
            OnStart += OnPipelineStart;

            OnComplete += OnPipelineComplete;

            var input = context.IO.GetInput("test_data");

            var output = new DebugOutput { Context = context };

            var batchId = Guid.NewGuid().ToString();

            var batchIdAction = new AttachBatchIdAction("BatchId", batchId) {  Context = context };

            input.LinkTo(batchIdAction);

            batchIdAction.Link.SuccessTo(output).End();

            EnsureComplete(output.Compltetion);

            return input;
        }

        private void OnPipelineComplete()
        {
            Console.WriteLine("Pipeline complete!");
        }

        protected void OnPipelineStart()
        {
            Console.WriteLine("Pipeline start...");
        }
    }
}
