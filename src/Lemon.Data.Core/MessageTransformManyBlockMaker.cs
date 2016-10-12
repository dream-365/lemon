using System;
using System.Collections.Generic;
using Lemon.Data.Core.Models;

namespace Lemon.Data.Core
{
    public class MessageTransformManyBlockMaker<TInput, TOutput>
    {
        private Func<TInput, IEnumerable<TOutput>> _func;

        public MessageTransformManyBlockMaker(Func<TInput, IEnumerable<TOutput>> func)
        {
            _func = func;
        }

        private IEnumerable<MessageWrapper<TOutput>> TransformManyImpl(MessageWrapper<TInput> messageWrapper)
        {
            var list = new List<MessageWrapper<TOutput>>();

            try
            {
                var messages = _func(messageWrapper.Message);

                if(messages != null)
                {
                    foreach(var msg in messages)
                    {
                        list.Add(new MessageWrapper<TOutput> { Message = msg, IsBroken = false });
                    }
                }
            }
            catch (Exception ex)
            {
                if(messageWrapper != null)
                {
                    LogService.Default.Error(string.Format("exception on pipeline {0}, value = {1}", messageWrapper.PipelineId, messageWrapper.Message), ex);
                }else
                {
                    LogService.Default.Error("empty message in transoform many", ex);
                }
            }

            return list;
        }

        public Func<MessageWrapper<TInput>, IEnumerable<MessageWrapper<TOutput>>> TransformMany {
            get { return TransformManyImpl; }
        }
    }
}
