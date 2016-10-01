using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lemon.Transform;
using Lemon.Transform.Models;

namespace Demo001
{
    public class ProgressAwarePipeline : DataFlowPipeline
    {
        protected override AbstractDataInput OnCreate(PipelineContext context)
        {
            var input = context.Attach(new JsonFileDataInput("test_data.json"));

            var debug = context.Attach(new DebugOutput(), "debug_output");

            var output = context.IO.GetOutput("sql_data_output");

            var broadCast = new BroadcastAction();

            input.Link.SuccessTo(broadCast).End();

            broadCast.Link.SuccessTo(debug);

            broadCast.Link.SuccessTo(output);

            Waits(output);

            return input;
        }

        protected override void 
            OnProgressChange(IEnumerable<ProgressStateItem> progress)
        {
            var dict = new Dictionary<string, string>();

            foreach(var groupItem in progress.GroupBy(m => m.ActionName))
            {
                var sb = new StringBuilder();

                sb.Append("(");

                var first = groupItem.First();

                sb.AppendFormat("{0}={1}", first.PortName, first.Count);

                foreach (ProgressStateItem item in groupItem.Skip(1))
                {
                    sb.AppendFormat(", {0}={1}", item.PortName, item.Count);
                }

                sb.Append(")");

                dict.Add(groupItem.Key, sb.ToString());
            }

            PrintState(dict, RootNode, "");

            base.OnProgressChange(progress);
        }

        /// <summary>
        /// Recursively printting the status
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="node"></param>
        /// <param name="indentation"></param>
        private void PrintState(IDictionary<string, string> dict, ConnectionNode node, string indentation)
        {
            try
            {
                var childIndentation = (node.Visible ? indentation + "  " : indentation);

                if(node.Visible)
                {
                    Console.WriteLine("{0}->{1}:{2}", indentation, node.Name, dict[node.Name]);
                }

                foreach (var childNode in node.ChildrenNodes)
                {
                    PrintState(dict, childNode, childIndentation);
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
