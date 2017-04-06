using System;
using Lemon.Core.Models;
using log4net;

namespace Lemon.Core
{
    internal class MessageTransformBlockMaker<TInput, TOuput>
    {
        private Func<TInput, TOuput> _func;
        private Func<MessageWrapper<TInput>, MessageWrapper<TOuput>> _transform;
        protected readonly ILog Logger;

        public MessageTransformBlockMaker(Func<TInput, TOuput> func)
        {
            _func = func;
            _transform = TransformImpl;
            Logger = LogService.Default.GetLog("MessageTransformBlock");
        }

        private MessageWrapper<TOuput> TransformImpl(MessageWrapper<TInput> messageWrapper)
        {
            try
            {
                return new MessageWrapper<TOuput> { Message =_func(messageWrapper.Message), IsBroken = false };
            }
            catch (Exception ex)
            {
                if(messageWrapper != null)
                {
                    Logger.Error(string.Format("exception on pipeline {0}, value = {1}", messageWrapper.PipelineId, messageWrapper.Message), ex);   
                }else
                {
                    Logger.Error("empty message - transform", ex);
                }

                return new MessageWrapper<TOuput> { IsBroken = true };
            }
        }

        public Func<MessageWrapper<TInput>, MessageWrapper<TOuput>> Transform { get { return _transform; } }
    }
}
