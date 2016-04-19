using eas.modules;
using Lemon.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eas.main
{
    public class NormalizeModule : IProcessModule
    {
        public void Start(object obj)
        {
            var process = new NormalizeProcess();

            process.OnErrorQueueName = ConfigurationManager.AppSettings["eas:onerror"];

            //process.SetBlobClient(new Lemon.Storage.AzureBlobClient(ConfigurationManager.AppSettings["eas:storage"]));
            process.SetBlobClient(new LocalBlobClient(ConfigurationManager.AppSettings["eas:storage"]));

            //process.SetMessageQueueProvider(new Lemon.Storage.Message.DefaultMessageQueueProvider());
            process.SetMessageQueueProvider(new MemoryMessageQueueProvider());

            process.SetPersistenceProvider(new Lemon.Storage.MongoDBPersistenceProvider());

            process.SetNormaliztionProvider(new EasDefaultModuleProvider());

            process.Start(ConfigurationManager.AppSettings["eas:normalize"]);
        }
    }
}
