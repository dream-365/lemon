using System;

namespace Lemon.Core.Message
{
    public enum MessageQueueType
    {
        MSMQ = 0,
        CloudMessageQueue = 1
    }

    public class MessageQueueProvider
    {
        private MessageQueueType _queueType = MessageQueueType.CloudMessageQueue;

        private MessageQueueProvider()
        {

        }

        public static MessageQueueProvider Current = new MessageQueueProvider();

        public void Use(MessageQueueType type)
        {
            _queueType = type;
        }

        public IMessageQueue GetMessageQueue(string name, bool createIfNotExists = true)
        {
            if(_queueType == MessageQueueType.MSMQ)
            {
                return new MSMQMessageQueue(name);
            }
            else if(_queueType == MessageQueueType.CloudMessageQueue)
            {
                return new CloudMessageQueue(name, createIfNotExists);
            }

            throw new NotImplementedException("the queue type is not supported!");
        }
    }
}
