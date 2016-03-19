using eas.modules;
using Lemon.Core;
using System.Configuration;

namespace eas.normalize
{
    class Program
    {
        static void Main(string[] args)
        {
            var process = new NormalizeProcess();

            process.OnErrorQueueName = ConfigurationManager.AppSettings["eas:onerror"];

            process.SetBlobClient(new Lemon.Storage.AzureBlobClient(ConfigurationManager.AppSettings["eas:storage"]));

            process.SetMessageQueueProvider(new Lemon.Storage.Message.DefaultMessageQueueProvider());

            process.SetPersistenceProvider(new Lemon.Storage.MongoDBPersistenceProvider());

            process.SetNormaliztionProvider(new EasDefaultModuleProvider());

            process.Start(ConfigurationManager.AppSettings["eas:normalize"]);
        }
    }
}
