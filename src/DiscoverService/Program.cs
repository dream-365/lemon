using Lemon.Core;
using Lemon.Core.Config;
using Lemon.Core.Discover;
using Lemon.Core.Message;
using Lemon.Core.Message.Body;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscoverService
{
    class Program
    {
        /// <summary>
        /// -name uwp_sort_by_post -collection collectionName [-setting http://www.setting.com]
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("command: -name uwp_sort_by_post -collection collectionName [-setting http://www.setting.com|localSettings.json(default)]");

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

            var settingName = parameters["name"];

            var collectionName = parameters["collection"];

            var configrationManager = new JsonConfigrationManager<GeneralXPatUriFindSetting>("localSettings.json");

            var setting = configrationManager.GetSetting(settingName);

            IDiscover discovery = new PaginationDiscovery(setting);

            var downloadDataQueue = MessageQueueProvider.Current.GetMessageQueue(GeneralSettings.DownloadDataQueueName);

            discovery.OnDiscovered += (url) =>
            {
                var cmd = new DownloadContentMessageBody
                {
                    Url = url,
                    SaveTo = collectionName
                };

                downloadDataQueue.Send(cmd);

                Console.WriteLine(url);
            };

            discovery.Start();

            downloadDataQueue.Close();
        }
    }
}
