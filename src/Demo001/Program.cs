using Lemon.Transform;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Demo001
{
    class Program
    {
        static void Main(string[] args)
        {
            GlobalConfiguration.Configure(config => {
                config.UseDefaultSevices();
            });

            var pipeline = new MongoToDebugDemo();

            var task = pipeline.RunAsync(new Dictionary<string, string> {
                {"scope", "powerbi" }
            });

            var timer = new Timer(10000);

            timer.AutoReset = true;

            timer.Elapsed += (sender, parameters) => {
                foreach (var kv in pipeline.GetAllProgress())
                {
                    Console.WriteLine("{0}: {1}", kv.Key, kv.Value);
                }
            };

            timer.Start();

            task.Wait();

            var status = task.Result;

            Console.WriteLine("Status: " + status);

            foreach (var kv in pipeline.GetAllProgress())
            {
                Console.WriteLine("{0}: {1}", kv.Key, kv.Value);
            }
        }
    }
}
