using System;

namespace Lemon.Transform
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
