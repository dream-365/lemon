using Microsoft.ServiceBus.Messaging;

namespace Lemon.Data.IO.Azure
{
    public class ServiceBusQueueBase
    {
        private readonly string _connectionString;
        private readonly string _queueName;
        protected QueueClient queueClient;

        public ServiceBusQueueBase(string connectionString, string queueName)
        {
            Assert.IsNotNullOrWhiteSpace(connectionString, "connectionString");
            Assert.IsNotNullOrWhiteSpace(queueName, "queueName");

            _connectionString = connectionString;
            _queueName = queueName;

            Initialize();
        }

        private void Initialize()
        {
            queueClient = QueueClient.CreateFromConnectionString(_connectionString, _queueName);
        }
    }
}
