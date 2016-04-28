using Lemon.Core.Model;
using System;
using System.Collections.Generic;

namespace Lemon.Core.Process
{
    public class ListDiscoverProcess
    {
        private IEnumerable<string> _resources;

        private IMessageQueueProvider _messageQueueProvider;

        public string DispatchQueueName { get; set; }

        public void SetMessageQueueProvider(IMessageQueueProvider provider)
        {
            _messageQueueProvider = provider;
        }

        public ListDiscoverProcess(IEnumerable<string> resources)
        {
            _resources = resources;
        }

        public void Start(string collection, string handler)
        {
            IMessageQueue dispatchQueue = _messageQueueProvider != null ? _messageQueueProvider.Get(DispatchQueueName, true) : null;

            foreach(var resource in _resources)
            {
                if (dispatchQueue != null)
                {
                    var message = new DownloadContentMessageBody
                    {
                        Url = resource
                    };

                    message.Context.Add("saveTo", collection);

                    message.Context.Add("handler", handler);

                    dispatchQueue.Send(message);

                    Console.WriteLine("[Dispatch]");
                }
            }
        }
    }
}
