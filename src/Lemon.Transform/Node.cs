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

    public class SourceNode : Node
    {
        public SourceNode()
        {
            NodeType = NodeType.SourceNode;
        }

        public IDataReader<BsonDataRow> Reader { get; set; }

        public Node Next { get; set; }
    }

    public class TransformNode : Node
    {
        public TransformNode ()
        {
            NodeType = NodeType.TransformNode;
        }

        public Func<DataRowTransformWrapper<BsonDataRow>, DataRowTransformWrapper<BsonDataRow>> Block;

        public Node Prev { get; set; }

        public Node Next { get; set; }
    }

    public class TransformManyNode : Node
    {
        public TransformManyNode()
        {
            NodeType = NodeType.TransformManyNode;
        }

        public Func<DataRowTransformWrapper<BsonDataRow>, IEnumerable<DataRowTransformWrapper<BsonDataRow>>> Block;

        public Node Prev { get; set; }

        public Node Next { get; set; }
    }

    public class ActionNode : Node
    {
        public Node Prev { get; set; }

        public ActionNode ()
        {
            NodeType = NodeType.ActionNode;
        }

        public IDataWriter<BsonDataRow> Writer { get; set; }
    }

    public class BroadCastNode : Node
    {
        private IList<Node> _nodes;

        public BroadCastNode()
        {
            NodeType = NodeType.BroadCastNode;

            _nodes = new List<Node>();
        }

        public void AddChild(Node node)
        {
            _nodes.Add(node);
        }

        public Node Prev { get; set; }

        public IEnumerable<Node> ChildNodes { get { return _nodes; } }
    }
}
