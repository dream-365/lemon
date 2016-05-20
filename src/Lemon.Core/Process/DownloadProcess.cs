using Lemon.Core;
using Lemon.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Core
{
    public class DownloadProcess
    {
        private IMessageQueueProvider _messageQueueProvider;

        private IBlobClient _blobClient;

        public string DispatchQueueName { get; set; }

        public void SetMessageQueueProvider(IMessageQueueProvider provider)
        {
            _messageQueueProvider = provider;
        }

        public void SetBlobClient(IBlobClient blobClient)
        {
            _blobClient = blobClient;
        }

        public void Start(string listenToQueue)
        {
            IMessageQueue downloadQueue = _messageQueueProvider.Get(listenToQueue, true);

            if(downloadQueue == null)
            {
                return;
            }

            IMessageQueue dispatchQueue = _messageQueueProvider.Get(DispatchQueueName, true);

            var httpClient = new HttpClient();

            while (true)
            {

                try
                {
                    var message = downloadQueue.Dequeue<DownloadContentMessageBody>();

                    if (message != null)
                    {
                        Console.Write(message.Url);

                        var task = httpClient.GetStreamAsync(message.Url);

                        task.Wait();

                        Console.Write("[download]");

                        var generatedPath = string.Format("{0}/{1}", DateTime.Now.ToString("yyyy-MM-dd"), Guid.NewGuid());

                        _blobClient.Upload(task.Result, generatedPath);

                        Console.Write("[store]");

                        dispatchQueue.Send(new ProcessContentMessageBody
                        {
                            OrignalUrl = message.Url,
                            BlobPath = generatedPath,
                            Context = message.Context
                        });

                        Console.WriteLine();
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(10000);
                    }
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
