using Lemon.Transform.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Lemon.Transform
{
    public class Pipeline
    {
        private Node _root;

        public int BoundedCapacity { get; set; }

        public TransformActionChain Source(IDataReader<BsonDataRow> reader)
        {
            _root = new SourceNode
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

            var sourceNode = _root as SourceNode;

            var bufferBlock = new BufferBlock<DataRowTransformWrapper<BsonDataRow>>(new DataflowBlockOptions { BoundedCapacity = BoundedCapacity });

            var tasks = new List<Task>();

            var target = BuildTargetBlock(sourceNode.Next, tasks);

            bufferBlock.LinkTo(target, new DataflowLinkOptions { PropagateCompletion = true }, m => m.Success);

            var reader = sourceNode.Reader;

            return new Execution(async (parameters) => {
                while (!reader.End)
                {
                    var row = reader.Read();

                    await bufferBlock.SendAsync(new DataRowTransformWrapper<BsonDataRow> { Row = row, Success = true });
                }

                bufferBlock.Complete();

                await Task.Run(() =>
                {
                    Task.WaitAll(tasks.ToArray());

                    return true;
                });
              
                return true;
            });
        }

        private ITargetBlock<DataRowTransformWrapper<BsonDataRow>> BuildTargetBlock(Node node, IList<Task> tasks)
        {
            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

            var executionOptions = new ExecutionDataflowBlockOptions
            {
                BoundedCapacity = BoundedCapacity
            };

            if(node.NodeType == NodeType.ActionNode)
            {
                var actionNode = node as ActionNode;

                var actionBlock = new ActionBlock<DataRowTransformWrapper<BsonDataRow>>(item => actionNode.Writer.Write(item.Row), executionOptions);

                tasks.Add(actionBlock.Completion);

                return actionBlock;
            }
            else if (node.NodeType == NodeType.TransformNode)
            {
                var transformNode = node as TransformNode;

                var transformBlock = new TransformBlock<DataRowTransformWrapper<BsonDataRow>, DataRowTransformWrapper<BsonDataRow>>(transformNode.Block, executionOptions);

                var next = BuildTargetBlock(transformNode.Next, tasks);

                transformBlock.LinkTo(next, linkOptions, m => m.Success);

                return transformBlock;
            }
            else if (node.NodeType == NodeType.TransformManyNode)
            {
                var transformManyNode = node as TransformManyNode;

                var transformManyBlock = new TransformManyBlock<DataRowTransformWrapper<BsonDataRow>, DataRowTransformWrapper<BsonDataRow>>(transformManyNode.Block, executionOptions);

                var next = BuildTargetBlock(transformManyNode.Next, tasks);

                transformManyBlock.LinkTo(next, linkOptions, m => m.Success);

                return transformManyBlock;
            }
            else if (node.NodeType == NodeType.BroadCastNode)
            {
                var broadcastNode = node as BroadCastNode;

                IList<ITargetBlock<DataRowTransformWrapper<BsonDataRow>>> targets = new List<ITargetBlock<DataRowTransformWrapper<BsonDataRow>>>(); 

                foreach(var childNode in broadcastNode.ChildNodes)
                {
                    targets.Add(BuildTargetBlock(childNode, tasks));
                }

                var actionBlock = new ActionBlock<DataRowTransformWrapper<BsonDataRow>>(async item => {
                    foreach (var target in targets)
                    {
                        await target.SendAsync(item);
                    }
                }, executionOptions);

                actionBlock.Completion.ContinueWith(task => {
                    foreach (var target in targets)
                    {
                        target.Complete();
                    }
                });

                return actionBlock;
            } else
            {
                throw new Exception("the node type does not support buidling target block");
            }
        }
    }
}
