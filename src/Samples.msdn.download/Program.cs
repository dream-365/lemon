using Lemon.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples.msdn.download
{
    class Program
    {
        static void Main(string[] args)
        {
            var process = new DownloadProcess();

            process.SetMessageQueueProvider(new Lemon.Storage.Message.DefaultMessageQueueProvider());

            process.SetBlobClient(new Lemon.Storage.AzureBlobClient("container-name"));

            process.Start("download-queue-name");
        }
    }
}
