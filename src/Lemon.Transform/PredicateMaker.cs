using System;
using Lemon.Transform.Models;

namespace Lemon.Transform
{
    public class MessagePredicateMaker<TMessage>
    {
        private bool IsNotBrokenImpl(MessageWrapper<TMessage> message)
        {
            return !message.IsBroken;
        }

        private bool IsBrokenImpl(MessageWrapper<TMessage> message)
        {
            return message.IsBroken;
        }

        public Predicate<MessageWrapper<TMessage>> IsNotBroken { get { return IsNotBrokenImpl; } }

        public Predicate<MessageWrapper<TMessage>> IsBroken { get { return IsBrokenImpl; } }
    }
}
