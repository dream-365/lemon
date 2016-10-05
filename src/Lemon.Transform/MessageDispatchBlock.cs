using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Transform
{
    public class MessageDispatchBlock<TMessage>
    {
        private Action<TMessage> _dispatch;

        private IEnumerable<DataflowBlockReflectionWrapper> _targets;

        public MessageDispatchBlock(IEnumerable<DataflowBlockReflectionWrapper> targets)
        {
            _targets = targets;

            _dispatch = DispatchImpl;
        }

        private void DispatchImpl(TMessage message)
        {
            foreach (var target in _targets)
            {
                target.SendAsync(message).Wait();
            }
        }

        public Action<TMessage> Dispatch { get { return _dispatch;  }  }
    }
}
