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
                config.RegisterServcie<IDataSourceService, JsonDataSourceService>();
                config.BoundedCapacity = 1000;
            });

            var pipeline = new OutOfMemoryDataPipeline();

            var status = pipeline.RunAsync().Result;
        }
    }
}
