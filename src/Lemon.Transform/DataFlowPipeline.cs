using System.Collections.Generic;

namespace Lemon.Transform
{
    public abstract class DataFlowPipeline
    {
        protected abstract AbstractDataInput OnCreate(IOContext context);

        public void Run(IDictionary<string, string> namedParameters = null)
        {
            var entry = OnCreate(new IOContext(namedParameters));

            entry.Start();
        }
    }
}
