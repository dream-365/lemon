using System;
using Lemon.Transform.Models;

namespace Lemon.Transform
{
    public class MessageTransformBlockMaker<TInput, TOuput>
    {
        private Func<TInput, TOuput> _func;

        private Func<MessageWrapper<TInput>, MessageWrapper<TOuput>> _transform;

        public MessageTransformBlockMaker(Func<TInput, TOuput> func)
        {
            _func = func;

            _transform = TransformImpl;
        }

        private MessageWrapper<TOuput> TransformImpl(MessageWrapper<TInput> messageWrapper)
        {
            try
            {
                return new MessageWrapper<TOuput> { Message =_func(messageWrapper.Message), IsBroken = false };
            }
            catch (Exception)
            {
                return new MessageWrapper<TOuput> { Message = default(TOuput), IsBroken = true };
            }
        }

        public Func<MessageWrapper<TInput>, MessageWrapper<TOuput>> Transform { get { return _transform; } }
    }
}
