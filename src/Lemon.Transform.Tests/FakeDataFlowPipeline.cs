using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Transform.Tests
{
    public class FakeDataFlowPipeline : DataFlowPipeline
    {
        protected override AbstractDataInput OnCreate(PipelineContext context)
        {
            var input = context.IO.GetInput("any");

            var action1 = new FakeTransformAction2();

            var output = new FakeDataOutput();

            input.LinkTo(action1);

            action1.Link.SuccessTo(output).End();

            return input;
        }
    }
}
