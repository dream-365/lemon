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

        public TransformActionChain Next(ITransformBlock block)
        {
            var transformNode = new TransformNode
            {
                Block = block.Transform
            };

            if (_current.NodeType == NodeType.SourceNode)
            {
                (_current as SourceNode).Next = transformNode;
                transformNode.Prev = _current;
            }
            else if (_current.NodeType == NodeType.TransformNode)
            {
                (_current as TransformNode).Next = transformNode;
                transformNode.Prev = _current;
            }
            else if (_current.NodeType == NodeType.TransformManyNode)
            {
                (_current as TransformManyNode).Next = transformNode;
                transformNode.Prev = _current;
            }
            else if (_current.NodeType == NodeType.BroadCastNode)
            {
                (_current as BroadCastNode).AddChild(transformNode);
                transformNode.Prev = _current;
            }
            else
            {
                throw new Exception("Node type does not support next");
            }

            _current = transformNode;

            return this;
        }

        public TransformActionChain NextToMany(ITransformManyBlock block)
        {
            var transformManyNode = new TransformManyNode
            {
                Block = block.Transform
            };

            if (_current.NodeType == NodeType.SourceNode)
            {
                (_current as SourceNode).Next = transformManyNode;
                transformManyNode.Prev = _current;
            }
            else if (_current.NodeType == NodeType.TransformNode)
            {
                (_current as TransformNode).Next = transformManyNode;
                transformManyNode.Prev = _current;
            }
            else if (_current.NodeType == NodeType.TransformManyNode)
            {
                (_current as TransformManyNode).Next = transformManyNode;
                transformManyNode.Prev = _current;
            }
            else if (_current.NodeType == NodeType.BroadCastNode)
            {
                (_current as BroadCastNode).AddChild(transformManyNode);
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

            if (_current.NodeType == NodeType.SourceNode)
            {
                (_current as SourceNode).Next = broadCastNode;
            }
            else if (_current.NodeType == NodeType.TransformNode)
            {
                (_current as TransformNode).Next = broadCastNode;
                broadCastNode.Prev = _current;
            }
            else if (_current.NodeType == NodeType.TransformManyNode)
            {
                (_current as TransformManyNode).Next = broadCastNode;
                broadCastNode.Prev = _current;
            }
            else
            {
                throw new Exception("Node type does not support broadcast");
            }

            return new BroadCastActionChain(broadCastNode);
        }

        public void Output(IDataWriter<BsonDataRow> writer)
        {
            var actionNode = new ActionNode
            {
                Writer = writer
            };

            if (_current.NodeType == NodeType.SourceNode)
            {
                (_current as SourceNode).Next = actionNode;
            }
            else if (_current.NodeType == NodeType.TransformNode)
            {
                (_current as TransformNode).Next = actionNode;
            }
            else if (_current.NodeType == NodeType.TransformManyNode)
            {
                (_current as TransformManyNode).Next = actionNode;
            }
            else if (_current.NodeType == NodeType.BroadCastNode)
            {
                (_current as BroadCastNode).AddChild(actionNode);
            }
            else
            {
                throw new Exception("Node type does not support output");
            }
        }
    }
}
