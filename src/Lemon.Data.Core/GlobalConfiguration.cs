using System;

namespace Lemon.Data.Core
{
    public class GlobalConfiguration
    {
        internal static TransformConfiguration TransformConfiguration = new TransformConfiguration();

        public static void Configure(Action<TransformConfiguration> config)
        {
            config(TransformConfiguration);
        }
    }
}
