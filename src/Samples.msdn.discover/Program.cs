using eas.modules;
using Lemon.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace eas.discover
{
    // discover.exe -name settingName
    class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // parse parameters
            if (args.Length < 2)
            {
                Console.WriteLine("-name settingName -collection collectinName -handler handlerName");

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

            string name;

            string collection;

            string handler;

            try
            {
                name = parameters["name"];

                collection = parameters["collection"];

                handler = parameters["handler"];
            }
            catch
            {
                Console.WriteLine("invliad parameters");

                return;
            }

            // load settings 
            var process = new DiscoverProcess();

            process.LoadSettings("disocver.settings.json");

            process.SetBuildIndexProivder(new EasBuildIndexProvider());

            //process.SetMessageQueueProvider(new DefaultMessageQueueProvider());
            //process.SetMessageQueueProvider(new MemoryMessageQueueProvider());

            //process.DispatchQueueName = ConfigurationManager.AppSettings["eas:download"];

            process.OnNew((item) => {
                Console.WriteLine(item.GetValue("uri").AsString);
                Console.WriteLine(item.GetValue("title").AsString);
                Console.WriteLine(item.GetValue("vote").AsInt32);

                var log = String.Format("{0} \t {1}\t {2}", item.GetValue("title").AsString, item.GetValue("uri").AsString, item.GetValue("vote").AsInt32);

                var path = "output.txt";

                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(log);
                    }
                }

                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(log);
                }

            });

            process.Start(name: name, collection: collection, handler: handler);
        }
    }
}
