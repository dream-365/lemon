using Lemon.Core;
using Lemon.Storage.Message;
using System;

namespace Samples.msdn.discover
{
    // discover.exe -name settingName
    class Program
    {
        static void Main(string[] args)
        {
            // load settings 
            var process = new DiscoverProcess();

            process.LoadSettings("disocver.settings.json");

            process.SetBuildIndexProivder(new SampleBuildIndexProvider());

            //process.SetMessageQueueProvider(new DefaultMessageQueueProvider());

            //process.DispatchQueueName = "msdn-download-queue";

            process.OnNew((item) => {
                Console.WriteLine(item.GetValue("title").AsString);
            });

            process.Start("uwp_forum", "landing.uwp_threads");
        }
    }
}
