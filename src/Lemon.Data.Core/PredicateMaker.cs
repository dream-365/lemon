using System;
using Lemon.Data.Core.Models;

namespace Lemon.Data.Core
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

        public bool IsNullImpl(MessageWrapper<TMessage> message)
        {
            return message == null;
        }

        public Predicate<MessageWrapper<TMessage>> IsNotBroken { get { return IsNotBrokenImpl; } }

        public Predicate<MessageWrapper<TMessage>> IsBroken { get { return IsBrokenImpl; } }

        public Predicate<MessageWrapper<TMessage>> IsNull { get { return IsNullImpl; } }
    }
}
