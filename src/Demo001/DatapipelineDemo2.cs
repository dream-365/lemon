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

            var output = context.IO.GetOutput("sql_tbl001");

            input.LinkTo(output);

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
