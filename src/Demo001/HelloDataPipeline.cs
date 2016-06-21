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

            var output1 = context.GetOutput("console_user_activity");

            var output2 = context.GetOutput("user_activity");

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
