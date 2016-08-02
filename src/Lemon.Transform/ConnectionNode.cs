using System;
using System.Collections.Generic;
using System.Linq;

namespace Lemon.Transform
{
    public class ConnectionNode : IComparable, IComparable<ConnectionNode>
    {
        private string _name;

        public bool Visible { get; set; }

        public ConnectionNode Parent { get; private set; }

        public string Name { get { return _name; } }

        private  ICollection<ConnectionNode> _childrenNodes { get; set; }

        public ConnectionNode(string name)
        {
            _childrenNodes = new SortedSet<ConnectionNode>();

            _name = name;
        }

        public IEnumerable<ConnectionNode> ChildrenNodes
        {
            get
            {
                return _childrenNodes.AsEnumerable();
            }
        }

        /// <summary>
        /// add child node to current node
        /// </summary>
        /// <param name="childNode"></param>
        /// <returns></returns>
        public ConnectionNode AddChildNode(ConnectionNode childNode)
        {
            childNode.Parent = this;

            _childrenNodes.Add(childNode);

            return childNode;
        }

        public int CompareTo(object obj)
        {
            if(obj == null)
            {
                return 1;
            }

            var node = obj as ConnectionNode;

            if(node == null)
            {
                throw new ArgumentException("Object is not a Connection Node");
            }

            return CompareTo(node);
        }

        public int CompareTo(ConnectionNode other)
        {
            return Name.CompareTo(other.Name);
        }
    }
}
