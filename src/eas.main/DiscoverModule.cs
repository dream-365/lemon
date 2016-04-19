using eas.modules;
using Lemon.Core;
using Lemon.Storage.Message;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eas.main
{
    public class DiscoverModule : IProcessModule
    {
        private string name;
        private string collection;
        private string handler;

        public DiscoverModule(string name, string collection, string handler)
        {
            this.name = name;
            this.collection = collection;
            this.handler = handler;
        }

        public void Start(object obj)
        {
            var process = new DiscoverProcess();

            process.LoadSettings("disocver.settings.json");

            process.SetBuildIndexProivder(new EasBuildIndexProvider());

            //process.SetMessageQueueProvider(new DefaultMessageQueueProvider());
            process.SetMessageQueueProvider(new MemoryMessageQueueProvider());

            process.DispatchQueueName = ConfigurationManager.AppSettings["eas:download"];

            process.OnNew((item) => {
                Console.WriteLine(item.GetValue("uri").AsString);
                Console.WriteLine(item.GetValue("title").AsString);
            });

            process.Start(name: name, collection: collection, handler: handler);
        }
    }
}
