using Lemon.Core;
using Lemon.Core.Message;
using Lemon.Core.Message.Body;
using Lemon.Core.Storage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace ProcessingService
{
    class Program
    {
        /// <summary>
        /// -module moduleName
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("-module moduleName");

                return;
            }

            var parameters = new Dictionary<string, string>();

            int start = 0;

            while (start < args.Length)
            {
                var pair = args.Skip(start).Take(2);

                parameters.Add(pair.ElementAt(0).TrimStart('-'), pair.ElementAt(1));

                start = start + 2;
            }

            var moduleName = parameters["module"];

            var assemblyFilePath = ConfigurationManager.AppSettings["assembly:modules"];

            var assembly = Assembly.LoadFrom(assemblyFile: assemblyFilePath);

            var factoryInterfaceType = typeof(IStreamProcessingModuleProvider);

            var defaultProviderType = assembly.GetTypes().Where(m => factoryInterfaceType.IsAssignableFrom(m)).FirstOrDefault();

            var provider = Activator.CreateInstance(defaultProviderType) as IStreamProcessingModuleProvider;

            var processQueue = MessageQueueProvider.Current.GetMessageQueue(GeneralSettings.ProcessDataQueueName);

            var processErrorQueue = MessageQueueProvider.Current.GetMessageQueue(GeneralSettings.ProcessErrorQueueName);

            var blobClient = new AzureBlobClient(GeneralSettings.StorageContainerName);

            var modules = new List<IStreamProcessingModule>();

            modules.Add(provider.Activate(moduleName));

            var streamProcessingPipeline = new StreamProcessingPipeline(modules);

            Console.ForegroundColor = ConsoleColor.Blue;

            while (true)
            {
                var message = processQueue.Dequeue<ProcessContentMessageBody>();

                try
                {                    
                    if (message != null)
                    {                       
                        using (var stream = blobClient.Download(message.BlobPath))
                        {
                            var metadata = streamProcessingPipeline.Process(stream);

                            var writter = new MongoDataWritter(message.SaveTo);

                            writter.Save(metadata);
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

                    processErrorQueue.Send(new DownloadContentMessageBody {
                        Url = message.OrignalUrl,
                        SaveTo = message.SaveTo
                    });

                    Console.ForegroundColor = ConsoleColor.Blue;
                }
            }
        }
    }
}
