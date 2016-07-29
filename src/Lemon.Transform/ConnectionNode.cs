using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Transform
{
    public class ConnectionNode
    {
        private string _name;

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
    }
}
