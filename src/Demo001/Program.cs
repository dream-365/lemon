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
            LemonTransform.UseDefaultSevices();

            var pipeline = new DatapipelineDemo2();

            pipeline.Run();

            foreach(var kv in pipeline.GetAllProgress())
            {
                Console.WriteLine("{0}: {1}", kv.Key, kv.Value);
            }
        }
    }
}
