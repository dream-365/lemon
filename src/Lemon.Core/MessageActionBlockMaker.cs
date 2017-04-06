using System;
using Lemon.Core.Models;
using log4net;

namespace Lemon.Core
{
    internal class MessageActionBlockMaker<TMessage>
    {
        protected readonly ILog Logger;
        private Action<TMessage> _action;

        public MessageActionBlockMaker(Action<TMessage> action)
        {
            _action = action;
            Logger = LogService.Default.GetLog("MessageActionBlock");
        }

        private void ActionImpl(MessageWrapper<TMessage> messageWrapper)
        {
            try
            {
                _action(messageWrapper.Message);
            }
            catch (Exception ex)
            {
                if(messageWrapper != null)
                {
                    Logger.Error(
                        string.Format("exception on pipeline {0}, value = {1}", messageWrapper.PipelineId, messageWrapper.Message), 
                        ex);
                }else
                {
                    Logger.Error("empty message - action", ex);
                }
            }
        }

        public Action<MessageWrapper<TMessage>> Action { get { return ActionImpl; } }
    }
}
