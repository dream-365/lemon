using Lemon.Transform;
using System.Collections.Generic;

namespace Demo001
{
    class Program
    {
        static void Main(string[] args)
        {
            LemonTransform.UseDefaultSevices();

            LemonTransform.RegisterDataOutput<ConsoleOutput>("console");

            var pipeline = new HelloDataPipeline();

            pipeline.Run(new Dictionary<string, string> {
                {"scope", "uwp"}
            });
        }
    }
}
