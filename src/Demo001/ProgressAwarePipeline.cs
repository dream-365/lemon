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
            throw new NotImplementedException();
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

        private void PrintState(IDictionary<string, string> dict, ConnectionNode node, string indentation)
        {
            Console.WriteLine("{0}:{1}", node.Name, dict[node.Name]);

            foreach(var childNode in node.ChildrenNodes)
            {
                PrintState(dict, childNode, indentation + "  ");
            }
        }
    }
}
