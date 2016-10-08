using Lemon.Transform.Models;
using System;
using System.Collections.Generic;

namespace Lemon.Transform
{
    public class MessageBroadCastBlockMaker<TMessage>
    {
        private Action<TMessage> _dispatch;

        private Type _messageType;

        private IEnumerable<DataflowBlockReflectionWrapper> _targets;

        public MessageBroadCastBlockMaker(IEnumerable<DataflowBlockReflectionWrapper> targets)
        {
            _targets = targets;

            _dispatch = DispatchImpl;

            _messageType = typeof(MessageWrapper<>).MakeGenericType(typeof(TMessage));
        }

        private void DispatchImpl(TMessage message)
        {
            foreach (var target in _targets)
            {
                var messageWrapper = Activator.CreateInstance(_messageType, new object[] { message });

                target.SendAsync(messageWrapper).Wait();
            }
        }

        public Action<TMessage> Dispatch { get { return _dispatch;  }  }
    }
}
 