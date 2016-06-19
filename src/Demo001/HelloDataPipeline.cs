using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lemon.Transform;

namespace Demo001
{
    public class HelloDataPipeline : DataFlowPipeline
    {
        protected override AbstractDataInput OnCreate(IOContext context)
        {
            var input = context.GetInput("landing_threads");

            var output = context.GetOutput("console_user_activity");

            var action1 = new UserActiviesTransformAction();

            input.LinkTo(action1);

            action1.LinkTo(output);

            return input;
        }
    }
}
