using Lemon.Transform;
using System;

namespace Demo001
{
    public class MongoToDebugDemo : DataFlowPipeline
    {
        protected override AbstractDataInput OnCreate(PipelineContext context)
        {
            var input = context.IO.GetInput("landing_questions");

            input.SetDefaultParameterValue("start", DateTime.Parse("2000-1-1"));
            input.SetDefaultParameterValue("start", DateTime.Now);

            var debug = new DebugOutput();

            input.LinkTo(debug);

            Waits(debug);

            return input;
        }
    }
}
