using System;
using Lemon.Transform;

namespace Demo001
{
    public class DataPipelineDemo3 : DataFlowPipeline
    {
        protected override AbstractDataInput OnCreate(PipelineContext context)
        {
            var userRanksInput = context.IO.GetInput("msdn_user_ranks");

            var debugOutput = new DebugOutput();

            userRanksInput.LinkTo(debugOutput);

            EnsureComplete(debugOutput.Compltetion);

            return userRanksInput;
        }
    }
}
