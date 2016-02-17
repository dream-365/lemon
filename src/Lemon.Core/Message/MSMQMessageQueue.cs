using System;
using msmq = System.Messaging;
using System.Text;

namespace Lemon.Core.Message
{
    public class MSMQMessageQueue : IMessageQueue
    {
        private msmq.MessageQueue _queue;

        public MSMQMessageQueue(string name)
        {
            _queue = new msmq.MessageQueue(name);
        }

        public void Close()
        {
            _queue.Dispose();
        }

        public T Dequeue<T>()
        {
            throw new NotImplementedException();
        }

        public bool Send<T>(T body)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(body);

            using (var msg = new msmq.Message())
            using (var mem = new System.IO.MemoryStream(Encoding.Default.GetBytes(json)))
            {
                msg.BodyStream = mem;

                _queue.Send(msg);
            }

            return true;
        }
    }
}
