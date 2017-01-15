using Lemon.Data.Core;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Collections.Generic;

namespace Lemon.Data.IO.Azure
{
    public class CloudQueueMessageWriter<T> : IDataWriter<T>
    {
        private CloudQueue _cloudQueue;
        private readonly string _connectionString;
        private readonly string _queueName;

        public CloudQueueMessageWriter(string connectionString, string queueName)
        {
            if(string.IsNullOrWhiteSpace(connectionString) || string.IsNullOrWhiteSpace(queueName))
            {
                throw new System.ArgumentNullException();
            }

            _connectionString = connectionString;
            _queueName = queueName;

            Initialize();
        }

        private void Initialize()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_connectionString);

            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            _cloudQueue = queueClient.GetQueueReference(_queueName);

            _cloudQueue.CreateIfNotExists();
        }

        public void Dispose()
        {
            
        }

        public void Write(IEnumerable<T> records)
        {
            if(records == null)
            {
                return;
            }

            foreach(var record in records)
            {
                Write(record);
            }
        }

        public void Write(T record)
        {
            _cloudQueue.AddMessage(ToMessage(record));
        }

        private static CloudQueueMessage ToMessage(T record)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(record);

            CloudQueueMessage message = new CloudQueueMessage(json);

            return message;
        }
    }
}
