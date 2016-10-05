using Lemon.Transform.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Linq;

namespace Lemon.Transform
{
    public class Pipeline
    {
        private Node _root;

        public int BoundedCapacity { get; set; }

        public TransformActionChain DataSource<TSource>(IDataReader<TSource> reader)
        {
            _root = new DataSourceNode<TSource>
            {
                Reader = reader
            };

            return new TransformActionChain(_root);
        }


        public IExecute Build()
        {
            if(_root.NodeType != NodeType.SourceNode)
            {
                throw new Exception("the root node must be source node");
            }

            var reader = _root.GetType().GetProperty("Reader").GetValue(_root) as IDataReader;

            var sourceNode = _root as ISource;

            var bufferBlock = BlockBuilder.CreateBufferBlock(sourceNode.SourceType, new DataflowBlockOptions { BoundedCapacity = BoundedCapacity });

            var tasks = new List<Task>();

            var target = BuildTargetBlock(sourceNode.Next, tasks);

            bufferBlock.LinkTo(target, new DataflowLinkOptions { PropagateCompletion = true });

            return new Execution(async (parameters) => {
                while (!reader.End())
                {
                    var row = reader.ReadObject();

                    await bufferBlock.SendAsync(row);
                }

                await Task.Run(() =>
                {
                    Task.WaitAll(tasks.ToArray());

                    return true;
                });
              
                return true;
            });
        }

        private object BuildTargetBlock(Node node, IList<Task> tasks)
        {
            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

            var executionOptions = new DataflowBlockOptions
            {
                BoundedCapacity = BoundedCapacity
            };

            if(node.NodeType == NodeType.ActionNode)
            {
                var target = node as ITarget;

                var nodeClass = typeof(ActionNode<>).MakeGenericType(target.TargetType);

                var actionBlockClass = typeof(ActionBlock<>).MakeGenericType(target.TargetType);

                var writeFunc = nodeClass.GetProperty("Write").GetValue(node);

                var actionBlock = BlockBuilder.CreateActionBlock(target.TargetType, writeFunc, new ExecutionDataflowBlockOptions {  BoundedCapacity = executionOptions.BoundedCapacity});

                tasks.Add(actionBlock.Completion);

                return actionBlock;
            }
            else if (node.NodeType == NodeType.TransformNode)
            {
                var source = node as ISource;

                var target = node as ITarget;

                var nodeClass = typeof(TransformNode<,>).MakeGenericType(source.SourceType, target.TargetType);

                var transformFunc = nodeClass.GetProperty("Block").GetValue(node);

                var transformBlock = BlockBuilder.CreateTransformBlock(source.SourceType, target.TargetType, transformFunc, new ExecutionDataflowBlockOptions {  BoundedCapacity = executionOptions.BoundedCapacity });

                var targetBlock = BuildTargetBlock(source.Next, tasks);

                transformBlock.LinkTo(targetBlock, new DataflowLinkOptions { PropagateCompletion = true });

                return transformBlock;
            }
             else
            {
                throw new Exception("the node type does not support buidling target block");
            }
        }
    }
}
