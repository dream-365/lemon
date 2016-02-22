using Lemon.Core;
using Lemon.Core.Message;
using Lemon.Core.Message.Body;
using Lemon.Core.Storage;
using System;
using System.Net.Http;

namespace DownloadService
{
    class Program
    {
        /// <summary>
        /// error
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            bool isError = false;

            if(args.Length > 0)
            {
                isError = args[0] == "error";
            }

            var downloadQueue = MessageQueueProvider.Current.GetMessageQueue(isError ? GeneralSettings.ProcessErrorQueueName : GeneralSettings.DownloadDataQueueName);

            var processQueue = MessageQueueProvider.Current.GetMessageQueue(GeneralSettings.ProcessDataQueueName);

            var httpClient = new HttpClient();

            var blobClient = new AzureBlobClient(GeneralSettings.StorageContainerName);

            while (true)
            {
                var data = downloadQueue.Dequeue<DownloadContentMessageBody>();

                if (data != null)
                {
                    Console.Write(data.Url);

                    var task = httpClient.GetStreamAsync(data.Url);

                    task.Wait();

                    Console.Write("[download]");

                    var generatedPath = string.Format("{0}/{1}", DateTime.Now.ToString("yyyy-MM-dd"), Guid.NewGuid());

                    blobClient.Upload(task.Result, generatedPath);

                    Console.Write("[store]");

                    processQueue.Send(new ProcessContentMessageBody
                    {
                        OrignalUrl = data.Url,
                        BlobPath = generatedPath,
                        SaveTo = data.SaveTo
                    });

                    Console.WriteLine();
                }
                else
                {
                    System.Threading.Thread.Sleep(10000);
                }
            }
        }
    }
}
