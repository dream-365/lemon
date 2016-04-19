using Lemon.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eas.main
{
    public class DownloadModule : IProcessModule
    {
        private bool onError = false;

        public DownloadModule(bool onError)
        {
            this.onError = onError;
        }
        
        public void Start(object obj)
        {
            var process = new DownloadProcess();

            //process.SetMessageQueueProvider(new Lemon.Storage.Message.DefaultMessageQueueProvider());
            process.SetMessageQueueProvider(new MemoryMessageQueueProvider());

            //process.SetBlobClient(new Lemon.Storage.AzureBlobClient(ConfigurationManager.AppSettings["eas:storage"]));
            process.SetBlobClient(new LocalBlobClient(ConfigurationManager.AppSettings["eas:storage"]));

            process.DispatchQueueName = ConfigurationManager.AppSettings["eas:normalize"];

            if (onError)
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
