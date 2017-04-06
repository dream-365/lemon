using Lemon.Core.Models;
using System;
using System.Collections.Generic;

namespace Lemon.Core
{
    internal class MessageBroadCastBlockMaker<TMessage>
    {
        private Action<TMessage> _dispatch;

        private Type _messageType;

        private Guid _pipelineId;

        private IEnumerable<DataflowBlockReflectionWrapper> _targets;

        public MessageBroadCastBlockMaker(IEnumerable<DataflowBlockReflectionWrapper> targets, Guid pipelineId)
        {
            _targets = targets;

            _pipelineId = pipelineId;

            _dispatch = DispatchImpl;

            _messageType = typeof(MessageWrapper<>).MakeGenericType(typeof(TMessage));
        }

        private void DispatchImpl(TMessage message)
        {
            foreach (var target in _targets)
            {
                var messageWrapper = Activator.CreateInstance(_messageType, new object[] { message, _pipelineId });

                target.SendAsync(messageWrapper).Wait();
            }
        }

        public Action<TMessage> Dispatch { get { return _dispatch;  }  }
    }
}
 