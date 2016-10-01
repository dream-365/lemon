using Lemon.Transform;
using LemonDemo;
using System;
using System.Collections.Generic;
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

            var action2 = new PrefixTransformBlock("b");

            var action3 = new PrefixTransformBlock("c");

            var writer1 = new ConsoleDataWriter("W1", 100);
            var writer2 = new ConsoleDataWriter("W2");

            var broadcast = pipeline.Source(new RandomDataReader(100))
                    .Next(action1)
                    .Next(action2)
                    .Next(action3)
                    .Broadcast();

            broadcast.Branch().Next(action1).Output(writer1);
            broadcast.Branch().Next(action2).Output(writer2);

            var exe = pipeline.Build();

            exe.RunAsync(null).Wait();
        }
    }
}
