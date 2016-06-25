using Lemon.Transform;
using System.Collections.Generic;

namespace Demo001
{
    class Program
    {
        static void Main(string[] args)
        {
            LemonTransform.UseDefaultSevices();

            var pipeline = new DataPipelineDemo3();

            pipeline.Run(new Dictionary<string, string> { { "startDate", "2016-6-17" }, { "endDate", "2016-6-24" } });

            //var pipeline = new HelloDataPipeline();

            //pipeline.Run(new Dictionary<string, string> {
            //    {"scope", "uwp"}
            //});

            pipeline.WaitForComplete();
        }
    }
}
