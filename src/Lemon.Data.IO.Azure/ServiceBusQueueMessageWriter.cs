using Lemon.Data.Core;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Collections.Generic;

namespace Lemon.Data.IO.Azure.IO
{
    public class ServiceBusQueueMessageWriter<T> : ServiceBusQueueBase, IDataWriter<T>
    {
        public ServiceBusQueueMessageWriter(string connectionString, string queueName) : base(connectionString, queueName)
        {
        }

        public void Dispose()
        {
            queueClient.Close();
        }

        public void Write(IEnumerable<T> records)
        {
            throw new NotImplementedException();
        }

        public void Write(T record)
        {
            Assert.IsNotNull(record, "record");
            var brokeredMessage = new BrokeredMessage(JsonConvert.SerializeObject(record));
            queueClient.Send(brokeredMessage);
        }
    }
}
