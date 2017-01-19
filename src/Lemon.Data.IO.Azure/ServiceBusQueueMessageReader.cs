using Lemon.Data.Core;
using Newtonsoft.Json;
using System;

namespace Lemon.Data.IO.Azure
{
    public class ServiceBusQueueMessage<T>
    {
        private Guid _lockToken;
        private T _data;
        public Guid LockToken { get { return _lockToken; } }
        public T Data { get { return _data; } }
        public ServiceBusQueueMessage(Guid lockToken, T data)
        {
            Assert.IsNotNull(lockToken, "lock token");
            Assert.IsNotNull(data, "data");
            _lockToken = lockToken;
            _data = data;
        }
        public override string ToString()
        {
            return _data != null ? _data.ToString() : "empty";
        }
    }

    public class ServiceBusQueueMessageReader<T> : ServiceBusQueueBase, IDataReader<ServiceBusQueueMessage<T>>
    {
        private ServiceBusQueueMessage<T> _message;
        private ServiceBusQueueMessgaeCommitter _committer;

        public ServiceBusQueueMessgaeCommitter Committer { get { return _committer;  } }

        public ServiceBusQueueMessageReader(string connectionString, string queueName) : base(connectionString, queueName)
        {
            Initialize();
        }

        private void Initialize()
        {
            _committer = new ServiceBusQueueMessgaeCommitter(queueClient);
        }

        public void Dispose()
        {
        }

        public bool Next()
        {
            try
            {
                var brokeredMessage = queueClient.Receive();

                if (brokeredMessage == null)
                {
                    return false;
                }

                _message = new ServiceBusQueueMessage<T>(
                    brokeredMessage.LockToken, 
                    JsonConvert.DeserializeObject<T>(brokeredMessage.GetBody<string>()));

                return true;
            }
            catch (Exception ex)
            {
                LogService.Default.Error("reading service bus queue failed", ex);
                return false;
            }
        }

        public ServiceBusQueueMessage<T> Read()
        {
            return _message;
        }

        object IDataReader.Read()
        {
            return Read();
        }
    }
}
