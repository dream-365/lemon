using System.Collections.Generic;

namespace Lemon.Core
{
    public class MemoryMessageQueueProvider : IMessageQueueProvider
    {
        private static IDictionary<string, IMessageQueue> QueueContainer = new Dictionary<string, IMessageQueue>();

        public IMessageQueue Get(string name, bool createIfNotExists)
        {
            if(QueueContainer.Keys.Contains(name))
            {
                return QueueContainer[name];
            }

            if(createIfNotExists)
            {
                var newQueue = new MemoryMessageQueue();

                QueueContainer.Add(name, newQueue);

                return newQueue;
            }

            return null;
        }
    }
}
