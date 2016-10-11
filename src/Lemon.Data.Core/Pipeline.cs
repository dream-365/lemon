using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Lemon.Data.Core.Models;

namespace Lemon.Data.Core
{
    public class Pipeline
    {
        private Node _root;

        public int BoundedCapacity { get; set; }

        private Guid _id;

        public Pipeline()
        {
            _id = Guid.NewGuid();
        }

        public TransformActionChain<TSource> DataSource<TSource>(IDataReader<TSource> reader)
        {
            _root = new DataSourceNode<TSource>
            {
                Reader = reader
            };

            return new TransformActionChain<TSource>(_root);
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

            var messageType = typeof(MessageWrapper<>).MakeGenericType(sourceNode.SourceType);

            var tasks = new List<Task>();

            var target = BuildTargetBlock(sourceNode.Next, tasks);

            bufferBlock.LinkTo(target, new DataflowLinkOptions { PropagateCompletion = true });

            return new Execution(async (parameters) => {
                while (!reader.End())
                {
                    try
                    {
                        var row = reader.ReadObject();

                        var message = Activator.CreateInstance(messageType, new object[] { row, _id });

                        var result = await bufferBlock.SendAsync(message);
                    }
                    catch (Exception)
                    {
                        // TODO: exception handle
                    }
                }

                reader.Dispose();

                bufferBlock.Complete();

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

            var executionOptions = new ExecutionDataflowBlockOptions
            {
                BoundedCapacity = BoundedCapacity
            };

            if(node.NodeType == NodeType.ActionNode)
            {
                var target = node as ITarget;

                var nodeClass = typeof(ActionNode<>).MakeGenericType(target.TargetType);

                var actionBlockClass = typeof(ActionBlock<>).MakeGenericType(target.TargetType);

                var writeFunc = nodeClass.GetProperty("Write").GetValue(node);

                var actionBlock = BlockBuilder.CreateActionBlock(target.TargetType, writeFunc, executionOptions);

                tasks.Add(actionBlock.Completion);

                return actionBlock;
            }
            else if(node.NodeType == NodeType.BroadCastNode)
            {
                var broadcast = node as IBroadCast;

                var target = node as ITarget;

                IList<DataflowBlockReflectionWrapper> targets = new List<DataflowBlockReflectionWrapper>();

                foreach(var childrenNode in broadcast.ChildrenNodes)
                {
                    targets.Add(new DataflowBlockReflectionWrapper(BuildTargetBlock(childrenNode, tasks)));
                }

                var dispatcherType = typeof(MessageBroadCastBlockMaker<>).MakeGenericType(target.TargetType);

                var dispatcher = Activator.CreateInstance(dispatcherType, new object[] { targets, _id });

                var dispatch = dispatcherType.GetProperty("Dispatch").GetValue(dispatcher);

                var actionBlock = BlockBuilder.CreateActionBlock(target.TargetType, dispatch, executionOptions);

                actionBlock.Completion.ContinueWith(task => {
                    foreach (var targetBlock in targets)
                    {
                        targetBlock.Complete();
                    }
                });

                return actionBlock; 
            }
            else if (node.NodeType == NodeType.TransformNode)
            {
                var source = node as ISource;

                var target = node as ITarget;

                var nodeClass = typeof(TransformNode<,>).MakeGenericType(source.SourceType, target.TargetType);

                var transformFunc = nodeClass.GetProperty("Block").GetValue(node);

                var options = new ExecutionDataflowBlockOptions
                {
                    BoundedCapacity = executionOptions.BoundedCapacity
                };

                if(node.MaxDegreeOfParallelism.HasValue)
                {
                    options.MaxDegreeOfParallelism = node.MaxDegreeOfParallelism.Value;
                }

                var transformBlock = BlockBuilder.CreateTransformBlock(source.SourceType, target.TargetType, transformFunc, options);

                var targetBlock = BuildTargetBlock(source.Next, tasks);

                transformBlock.LinkTo(targetBlock, linkOptions);

                return transformBlock;
            }
            else if (node.NodeType == NodeType.TransformManyNode)
            {
                var source = node as ISource;

                var target = node as ITarget;

                var nodeClass = typeof(TransformManyNode<,>).MakeGenericType(source.SourceType, target.TargetType);

                var transformManyFunc = nodeClass.GetProperty("Block").GetValue(node);

                var options = new ExecutionDataflowBlockOptions
                {
                    BoundedCapacity = executionOptions.BoundedCapacity
                };

                if (node.MaxDegreeOfParallelism.HasValue)
                {
                    options.MaxDegreeOfParallelism = node.MaxDegreeOfParallelism.Value;
                }

                var transformManyBlock = BlockBuilder.CreateTransformManyBlock(source.SourceType, target.TargetType, transformManyFunc, options);

                var targetBlock = BuildTargetBlock(source.Next, tasks);

                transformManyBlock.LinkTo(targetBlock, linkOptions);

                return transformManyBlock;
            }
            else
            {
                throw new Exception("the node type does not support buidling target block");
            }
        }
    }
}
