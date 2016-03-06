using Lemon.Core;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Storage.Message
{
    public class CloudMessageQueue : IMessageQueue
    {
        private static string _connectionString = ConfigurationManager.AppSettings.Get("StorageConnectionString");

        private CloudQueue _queue;

        public CloudMessageQueue(string name, bool createIfNotExists)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_connectionString);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            _queue = queueClient.GetQueueReference(name);
  
            if(createIfNotExists)
            {
                // Create the queue if it doesn't already exist.
                _queue.CreateIfNotExists();
            }
        }

        public void Close()
        {
            
        }

        public T Dequeue<T>()
        {
            var msg = _queue.GetMessage();

            if(msg == null)
            {
                return default(T);
            }

            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(msg.AsString);

            _queue.DeleteMessage(msg);

            return obj;
        }

        public bool Send<T>(T body)
        {
            var jsonFomatText = Newtonsoft.Json.JsonConvert.SerializeObject(body);

            // Create a message and add it to the queue.
            CloudQueueMessage message = new CloudQueueMessage(jsonFomatText);

            _queue.AddMessage(message);

            return true;
        }
    }
}
