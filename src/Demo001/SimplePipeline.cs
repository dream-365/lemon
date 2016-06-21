using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lemon.Transform;

namespace Demo001
{
    public class SimplePipeline : DataFlowPipeline
    {
        protected override AbstractDataInput OnCreate(IOContext context)
        {
            var input = context.GetInput("test_data");

            var consoleOutput = context.GetOutput("console_test");

            var mssqlOutput = context.GetOutput("sql_tbl001");

            var bad = new BadDataAction();

            var broadcast = new BroadcastAction();

            input.LinkTo(bad);

            bad.LinkTo(broadcast);

            broadcast.LinkTo(consoleOutput);

            broadcast.LinkTo(mssqlOutput);

            EnsureComplete(consoleOutput.Compltetion);

            EnsureComplete(mssqlOutput.Compltetion);

            return input;
        }
    }
}
