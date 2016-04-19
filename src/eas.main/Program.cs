using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eas.main
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 6)
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

            var modules = new List<IProcessModule>();
            modules.Add(new DiscoverModule(name, collection, handler));
            modules.Add(new DownloadModule(false));
            modules.Add(new NormalizeModule());

            modules.ForEach(foo => ThreadPool.QueueUserWorkItem(foo.Start));

            while (true)
            {
                System.Threading.Thread.Sleep(1000); // TODO: Break out of the loop
            }
        }
    }
}
