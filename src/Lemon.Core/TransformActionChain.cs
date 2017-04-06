using System;
using System.Collections.Generic;

namespace Lemon.Core
{
    public class TransformActionChain<TSource>
    {
        private Node _current;

        public TransformActionChain(Node current)
        {
            _current = current;
        }

        public TransformActionChain<TTarget> Transform<TTarget>(Func<TSource, TTarget> func, int? maxDegreeOfParallelism = null)
        {
            var transformNode = new TransformNode<TSource, TTarget>
            {
                Block = func,
                MaxDegreeOfParallelism = maxDegreeOfParallelism
            };

            if (_current.NodeType == NodeType.SourceNode ||
                _current.NodeType == NodeType.TransformNode ||
                _current.NodeType == NodeType.TransformManyNode
                )
            {
                (_current as ISource).Next = transformNode;
                transformNode.Prev = _current;
            }
            else
            {
                throw new Exception("Node type does not support next");
            }

            return new TransformActionChain<TTarget>(transformNode);
        }

        public TransformActionChain<TTarget> TransformMany<TTarget>(Func<TSource, IEnumerable<TTarget>> expression, int? maxDegreeOfParallelism = null)
        {
            var node = new TransformManyNode<TSource, TTarget> { Block = expression, MaxDegreeOfParallelism = maxDegreeOfParallelism };

            if (_current.NodeType == NodeType.SourceNode ||
                _current.NodeType == NodeType.TransformNode ||
                _current.NodeType == NodeType.TransformManyNode
                )
            {
                (_current as ISource).Next = node;
                node.Prev = _current;
            }
            else
            {
                throw new Exception("Node type does not support next");
            }

            return new TransformActionChain<TTarget>(node);
        }

        public TransformActionChain<TTarget> TransformMany<TTarget>(ITransformManyBlock<TSource, TTarget> block, int? maxDegreeOfParallelism = null)
        {
            return TransformMany(block.Transform, maxDegreeOfParallelism);
        }

        public TransformActionChain<TTarget> Transform<TTarget>(ITransformBlock<TSource, TTarget> block, int? maxDegreeOfParallelism = null)
        {
            return Transform(block.Transform, maxDegreeOfParallelism);
        }

        public void Output(IDataWriter<TSource> writer)
        {
            var actionNode = new ActionNode<TSource>
            {
                Write = writer.Write
            };

            if (_current.NodeType == NodeType.SourceNode ||
                _current.NodeType == NodeType.TransformNode ||
                _current.NodeType == NodeType.TransformManyNode)
            {
                (_current as ISource).Next = actionNode;
            }
            else
            {
                throw new Exception("Node type does not support output");
            }
        }
    }
}
