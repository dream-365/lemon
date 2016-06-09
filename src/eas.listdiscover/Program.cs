using Lemon.Core.Process;
using Lemon.Storage.Message;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eas.listdiscover
{
    class Program
    {
        static void Main(string[] args)
        {
            var process = new ListDiscoverProcess(new List<string> { "http://community.powerbi.com/t5/Desktop/Custom-Query-Error/m-p/40827/" });

            process.SetMessageQueueProvider(new DefaultMessageQueueProvider());

            process.DispatchQueueName = ConfigurationManager.AppSettings["eas:download"];

            process.Start(collection: "landing.powerbi_threads", handler: "powerbi");
        }
    }
}
