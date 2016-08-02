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

            var pipeline = new ProgressAwarePipeline();

            var task = pipeline.RunAsync(new Dictionary<string, object> {
                {"scope", "powerbi" }
            });

            task.Wait();

            Console.WriteLine("status: {0}", task.Result);
        }
    }
}
