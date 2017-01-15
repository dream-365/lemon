using System;
using Lemon.Data.Core;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace Lemon.Data.IO.Azure
{
    public class CloudQueueMessageReadModel<T>
    {
        private T _model;
        private CloudQueueMessage _message;

        public CloudQueueMessageReadModel(T model, CloudQueueMessage message)
        {
            _model = model;
            _message = message;
        }

        public T Model { get { return _model; } }

        public CloudQueueMessage Message { get { return _message;  } }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(Model);
        }
    }

    public class CloudQueueMessageReader<T> : IDataReader<CloudQueueMessageReadModel<T>>
    {
        private CloudQueue _queue;
        private CloudQueueMessageCommitter _commiter;
        private CloudQueueMessageReadModel<T> _current;
        private bool _poll;
        private readonly string _connectionString;
        private readonly string _queueName;
        private const int DEFAULT_POLL_SLEEP_SECONDS = 1;
        private const int DEFAULT_MESSAGE_LOCK_MINUTES = 5;

        public CloudQueueMessageReader(string connectionString, string queueName, bool poll = true)
        {
            if (string.IsNullOrWhiteSpace(connectionString) || string.IsNullOrWhiteSpace(queueName))
            {
                throw new ArgumentNullException();
            }

            _connectionString = connectionString;
            _queueName = queueName;

            _poll = poll;

            Initialzie();
        }

        private void Initialzie()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_connectionString);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            _queue = queueClient.GetQueueReference(_queueName);

            if(!_queue.Exists())
            {
                throw new Exception(string.Format("queue name = {0} does not exist", _queueName));
            }

            _commiter = new CloudQueueMessageCommitter(_queue);
        }

        public void Dispose()
        {
           
        }

        public bool Next()
        {
            var message = _queue.GetMessage();

            if(message == null && !_poll)
            {
                return false;
            }

            if (message == null)
            {
                message = Poll(TimeSpan.FromSeconds(DEFAULT_POLL_SLEEP_SECONDS));
            }

            _current = new CloudQueueMessageReadModel<T>(JsonConvert.DeserializeObject<T>(message.AsString), message);

            return true;
        }

        public CloudQueueMessageCommitter Committer { get { return _commiter;  } }

        private CloudQueueMessage Poll(TimeSpan period)
        {
            while(true)
            {
                System.Threading.Thread.Sleep(period);
                var message = _queue.GetMessage(TimeSpan.FromMinutes(DEFAULT_MESSAGE_LOCK_MINUTES));
                if(message != null)
                {
                    return message;
                }
            }
        }

        public CloudQueueMessageReadModel<T> Read()
        {
            return _current;
        }

        object IDataReader.Read()
        {
            return Read();
        }
    }
}
