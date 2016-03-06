using Example.Modules;
using Lemon.Core;

namespace Samples.msdn.normalize
{
    class Program
    {
        static void Main(string[] args)
        {
            var process = new NormalizeProcess();

            process.OnErrorQueueName = "on-error-queue-name";

            process.SetBlobClient(new Lemon.Storage.AzureBlobClient("container-name"));

            process.SetMessageQueueProvider(new Lemon.Storage.Message.DefaultMessageQueueProvider());

            process.SetPersistenceProvider(new Lemon.Storage.MongoDBPersistenceProvider());

            process.SetNormaliztionProvider(new DefaultModuleProvider());

            process.Start("listen-to-queue");
        }
    }
}
