using Microsoft.ServiceBus.Messaging;
using System;

namespace Lemon.Data.IO.Azure
{
    public class ServiceBusQueueMessgaeCommitter
    {
        private QueueClient _queueClient;

        public ServiceBusQueueMessgaeCommitter(QueueClient queueClient)
        {
            Assert.IsNotNull(queueClient, "queueClient");
            _queueClient = queueClient;
        }

        public void Commit(Guid lockToken)
        {
            _queueClient.Complete(lockToken);
        }
    }
}
