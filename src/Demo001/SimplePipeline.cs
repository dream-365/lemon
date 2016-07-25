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
        protected override AbstractDataInput OnCreate(PipelineContext context)
        {
            var input = context.IO.GetInput("test_data");

            var consoleOutput = context.IO.GetOutput("console_test");

            var mssqlOutput = context.IO.GetOutput("sql_tbl001");

            var bad = new BadDataAction();

            var broadcast = new BroadcastAction();

            input.LinkTo(bad);

            bad.Link.SuccessTo(broadcast).End();

            broadcast.Link.SuccessTo(consoleOutput).End();

            broadcast.Link.SuccessTo(mssqlOutput).End();

            Waits(consoleOutput, mssqlOutput);

            return input;
        }
    }
}
