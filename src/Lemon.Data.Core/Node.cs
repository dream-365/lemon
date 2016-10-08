using Lemon.Transform;
using Lemon.Transform.Models;
using System;
using System.Collections.Generic;

namespace Lemon.Transform
{
    public enum NodeType
    {
        SourceNode = 0,
        ActionNode = 1,
        TransformNode = 2,
        TransformManyNode = 3,
        BroadCastNode = 4
    }

    public enum TransformMode
    {
        OneToOne = 0,
        OneToMany = 1
    }

    public class Node
    {
        public NodeType NodeType { get; protected set; }
    }

    public class DataSourceNode<TSource> : Node, ISource
    {
        private Type _sourceType;

        public DataSourceNode()
        {
            NodeType = NodeType.SourceNode;

            _sourceType = typeof(TSource);
        }

        public IDataReader Reader { get; set; }

        public Node Next { get; set; }

        public Type SourceType
        {
            get
            {
                return _sourceType;
            }
        }
    }

    public class TransformNode<TSource, TTarget> : Node, ISource, ITarget
    {
        private Type _souceType;

        private Type _targetType;

        public TransformNode ()
        {
            NodeType = NodeType.TransformNode;

            _souceType = typeof(TSource);

            _targetType = typeof(TTarget);
        }

        public Func<TSource, TTarget> Block { get; set; }

        public Func<TSource, TTarget> GetBlock()
        {
            return Block;
        }

        public Node Prev { get; set; }

        public Node Next { get; set; }

        public Type SourceType
        {
            get
            {
                return _souceType;
            }
        }

        public Type TargetType
        {
            get
            {
                return _targetType;
            }
        }
    }

    public class TransformManyNode<TSource, TTarget> : Node, ISource, ITarget
    {
        private Type _souceType;

        private Type _targetType;

        public TransformManyNode()
        {
            NodeType = NodeType.TransformManyNode;

            _souceType = typeof(TSource);

            _targetType = typeof(TTarget);
        }

        public Func<TSource, IEnumerable<TTarget>> Block;

        public Node Prev { get; set; }

        public Node Next { get; set; }

        public Type SourceType
        {
            get
            {
                return _souceType;
            }
        }

        public Type TargetType
        {
            get
            {
                return _targetType;
            }
        }
    }

    public class ActionNode<TTarget> : Node, ITarget
    {
        private Type _targetType;

        public Node Prev { get; set; }

        public ActionNode ()
        {
            NodeType = NodeType.ActionNode;

            _targetType = typeof(TTarget);
        }

        public Action<TTarget> Write { get; set; }

        public Type TargetType
        {
            get
            {
                return _targetType;
            }
        }
    }

    public class BroadCastNode<TTarget> : Node, IBroadCast, ITarget
    {
        private IList<Node> _nodes;

        private Type _targetType;

        public BroadCastNode()
        {
            NodeType = NodeType.BroadCastNode;

            _targetType = typeof(TTarget);

            _nodes = new List<Node>();
        }

        public void AddChild(Node node)
        {
            _nodes.Add(node);
        }

        public Node Prev { get; set; }

        public IEnumerable<Node> ChildrenNodes
        {
            get
            {
                return _nodes;
            }
        }

        public Type TargetType
        {
            get
            {
                return _targetType;
            }
        }
    }
}
