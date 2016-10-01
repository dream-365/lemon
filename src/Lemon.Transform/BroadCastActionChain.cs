using Lemon.Transform.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;

namespace Lemon.Transform
{
    public class BroadCastActionChain
    {
        private BroadCastNode _node;

        public BroadCastActionChain(BroadCastNode node)
        {
            _node = node;
        }

        public TransformActionChain Branch()
        {
            return new TransformActionChain(_node);
        }
    }
}
