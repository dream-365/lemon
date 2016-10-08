using System;
using Lemon.Transform.Models;

namespace Lemon.Transform
{
    public class MessageActionBlockMaker<TMessage>
    {
        private Action<TMessage> _action;

        public MessageActionBlockMaker(Action<TMessage> action)
        {
            _action = action;
        }

        private void ActionImpl(MessageWrapper<TMessage> messageWrapper)
        {
            try
            {
                _action(messageWrapper.Message);
            }
            catch (Exception ex)
            {
                LogService.Default.Error(string.Format("exception on pipeline {0}, value = {1}", messageWrapper.PipelineId, messageWrapper.Message), ex);
            }
        }

        public Action<MessageWrapper<TMessage>> Action { get { return ActionImpl; } }
    }
}
