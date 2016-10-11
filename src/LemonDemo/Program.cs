using Lemon.Data.Core;
using LemonDemo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace Demo001
{
    class Program
    {
        static void Main(string[] args)
        {
            var pipeline = new Pipeline();

            pipeline.BoundedCapacity = 5;

            var action1 = new PrefixTransformBlock("a");

            var writer1 = new ConsoleDataWriter<string>("W1", 100);
            var writer2 = new ConsoleDataWriter<string>("W2");

            var broadcast = pipeline.DataSource(new RandomDataReader(100))
                    .Transform(action1, 10)
                    .TransformMany((line => {
                        Console.WriteLine("Thread Id: {0}", System.Threading.Thread.CurrentThread.ManagedThreadId);

                        var lines = new List<string>();

                        for(int i = 0; i < 3; i++)
                        {
                            lines.Add(string.Format("{0}_{1}", i, line));
                        }

                        return lines;
                    }), 10).Broadcast();

            broadcast.Branch().Output(writer1);
            broadcast.Branch().Output(writer2);

            var exe = pipeline.Build();

            exe.RunAsync(null).Wait();
        }
    }
}
