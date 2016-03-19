using Lemon.Core;
using System.Configuration;

namespace eas.download
{
    class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var process = new DownloadProcess();

            process.SetMessageQueueProvider(new Lemon.Storage.Message.DefaultMessageQueueProvider());

            process.SetBlobClient(new Lemon.Storage.AzureBlobClient(ConfigurationManager.AppSettings["eas:storage"]));

            process.DispatchQueueName = ConfigurationManager.AppSettings["eas:normalize"];

            if (args.Length > 0 && args[0] == "onerror")
            {
                process.Start(ConfigurationManager.AppSettings["eas:onerror"]);
            }
            else
            {
                process.Start(ConfigurationManager.AppSettings["eas:download"]);
            }
        }
    }
}
