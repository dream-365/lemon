using Lemon.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Core
{
    public class NormalizeProcess
    {
        private IMessageQueueProvider _messageQueueProvider;

        private IStreamProcessingModuleProvider _streamProcessingModuleProvider;

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

        public void SetStreamProcessingProvider(IStreamProcessingModuleProvider provider)
        {
            _streamProcessingModuleProvider = provider;
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
                            var modules = new List<IStreamProcessingModule>();

                            modules.Add(_streamProcessingModuleProvider.Activate(message.Context["handler"]));

                            var streamProcessingPipeline = new StreamProcessingPipeline(modules);

                            var metadata = streamProcessingPipeline.Process(stream);

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
                catch (Exception ex)
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
