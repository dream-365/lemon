using Lemon.Core.Model;
using System;
using System.Collections.Generic;

namespace Lemon.Core
{
    public class NormalizeProcess
    {
        private IMessageQueueProvider _messageQueueProvider;

        private INormaliztionProvider _normaliztionProvider;

        private IPersistenceProvider _persistenceProvider;

        private IBlobClient _blobClient;

        public string OnErrorQueueName { get; set; }

        public void SetMessageQueueProvider(IMessageQueueProvider provider)
        {
            _messageQueueProvider = provider;
        }

        public void SetBlobClient(IBlobClient blobClient)
        {
            _blobClient = blobClient;
        }

        public void SetPersistenceProvider(IPersistenceProvider provider)
        {
            _persistenceProvider = provider;
        }

        public void SetNormaliztionProvider(INormaliztionProvider provider)
        {
            _normaliztionProvider = provider;
        }

        public void Start(string listenToQueueName)
        {
            var processQueue = _messageQueueProvider.Get(listenToQueueName, false);

            if(processQueue == null)
            {
                return;
            }

            var processErrorQueue = _messageQueueProvider.Get(OnErrorQueueName, true);

            Console.ForegroundColor = ConsoleColor.Blue;

            var documentPersistenceCache = new Dictionary<string, IDocumentPersistence>();

            while (true)
            {
                var message = processQueue.Dequeue<ProcessContentMessageBody>();

                try
                {
                    if (message != null)
                    {
                        using (var stream = _blobClient.Download(message.BlobPath))
                        {
                            var module = _normaliztionProvider.Activate(message.Context["handler"]);

                            var metadata = module.Normalize(stream);

                            if (!documentPersistenceCache.ContainsKey(message.Context["saveTo"]))
                            {
                                documentPersistenceCache[message.Context["saveTo"]] = _persistenceProvider.Get(message.Context["saveTo"]);
                            }

                            IDocumentPersistence persistence = documentPersistenceCache[message.Context["saveTo"]];

                            persistence.Persist(metadata);
                        }

                        Console.Write(".");
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(10000);
                    }
                }
                catch (System.Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;

                    Console.Write("x");

                    Console.WriteLine(ex.Message);

                    processErrorQueue.Send(new DownloadContentMessageBody
                    {
                        Url = message.OrignalUrl,
                        Context = message.Context
                    });

                    Console.ForegroundColor = ConsoleColor.Blue;
                }
            }
        }
    }
}
