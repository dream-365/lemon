using Lemon.Transform;
using System;
using System.Timers;

namespace Demo001
{
    class Program
    {
        static void Main(string[] args)
        {
            LemonTransform.UseDefaultSevices();

            var pipeline = new UpdateOnChangeDemo();

            var task = pipeline.RunAsync();

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

            foreach (var kv in pipeline.GetAllProgress())
            {
                Console.WriteLine("{0}: {1}", kv.Key, kv.Value);
            }
        }
    }
}
