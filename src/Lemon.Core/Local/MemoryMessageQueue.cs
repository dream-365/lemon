using System.Collections.Concurrent;

namespace Lemon.Core
{
    public class MemoryMessageQueue : IMessageQueue
    {
        private ConcurrentQueue<string> _queue = new ConcurrentQueue<string>();

        public void Close()
        {
            
        }

        public T Dequeue<T>()
        {
            string text;

            if(_queue.TryDequeue(out text))
            {
               return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(text);
            }

            return default(T);
        }

        public bool Send<T>(T body)
        {
            var jsonFomatText = Newtonsoft.Json.JsonConvert.SerializeObject(body);

            _queue.Enqueue(jsonFomatText);

            return true;
        }
    }
}
