using Microsoft.WindowsAzure.Storage.Queue;
using System;

namespace Lemon.Data.IO.Azure
{
    public class CloudQueueMessageCommitter
    {
        private CloudQueue _queue;

        public CloudQueueMessageCommitter(CloudQueue queue)
        {
            if (queue == null)
            {
                throw new ArgumentNullException();
            }

            _queue = queue;
        }

        public void Commit(CloudQueueMessage record)
        {
            _queue.DeleteMessage(record);
        }
    }
}
