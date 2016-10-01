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
            });

            var pipeline = new OutOfMemoryDataPipeline();

            pipeline.Run();
        }
    }
}
