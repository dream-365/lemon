using Lemon.Transform.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;

namespace Lemon.Transform
{
    public class TransformActionChain
    {
        private Node _current;

        public TransformActionChain(Node current)
        {
            _current = current;
        }

        public TransformActionChain Next<TSource, TTarget>(ITransformBlock<TSource, TTarget> block)
        {
            var transformNode = new TransformNode<TSource, TTarget>
            {
                Block = block.Transform
            };

            if (_current.NodeType == NodeType.SourceNode ||
                _current.NodeType == NodeType.TransformNode ||
                _current.NodeType == NodeType.TransformManyNode
                )
            {
                (_current as ISource).Next = transformNode;
                transformNode.Prev = _current;
            }
            else if (_current.NodeType == NodeType.BroadCastNode)
            {
                (_current as IBroadCast).AddChild(transformNode);
                transformNode.Prev = _current;
            }
            else
            {
                throw new Exception("Node type does not support next");
            }

            _current = transformNode;

            return this;
        }

        public TransformActionChain NextToMany<TSource, TTarget>(ITransformManyBlock<TSource, TTarget> block)
        {
            var transformManyNode = new TransformManyNode<TSource, TTarget>
            {
                Block = block.Transform
            };

            if (_current.NodeType == NodeType.SourceNode ||
                _current.NodeType == NodeType.TransformNode ||
                _current.NodeType == NodeType.TransformManyNode)
            {
                (_current as ISource).Next = transformManyNode;
                transformManyNode.Prev = _current;
            }
            else if (_current.NodeType == NodeType.BroadCastNode)
            {
                (_current as IBroadCast).AddChild(transformManyNode);
                transformManyNode.Prev = _current;
            }
            else
            {
                throw new Exception("Node type does not support next");
            }

            _current = transformManyNode;

            return this;
        }

        public BroadCastActionChain Broadcast()
        {
            var broadCastNode = new BroadCastNode();

            if (_current.NodeType == NodeType.SourceNode || 
                _current.NodeType == NodeType.TransformNode ||
                _current.NodeType == NodeType.TransformManyNode)
            {
                (_current as ISource).Next = broadCastNode;
                broadCastNode.Prev = _current;
            }
            else
            {
                throw new Exception("Node type does not support broadcast");
            }

            return new BroadCastActionChain(broadCastNode);
        }

        public void Output<TTarget>(IDataWriter<TTarget> writer)
        {
            var actionNode = new ActionNode<TTarget>
            {
                Write = writer.Write
            };

            if (_current.NodeType == NodeType.SourceNode ||
                _current.NodeType == NodeType.TransformNode ||
                _current.NodeType == NodeType.TransformManyNode)
            {
                (_current as ISource).Next = actionNode;
            }
            else if (_current.NodeType == NodeType.BroadCastNode)
            {
                (_current as IBroadCast).AddChild(actionNode);
            }
            else
            {
                throw new Exception("Node type does not support output");
            }
        }
    }
}
