using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Lemon.Transform
{
    public abstract class DataFlowPipeline
    {
        private IList<Task> _compltetions;


        public DataFlowPipeline()
        {
            _compltetions = new List<Task>();
        }

        protected void EnsureComplete(Task completion)
        {
            _compltetions.Add(completion);
        }

        protected abstract AbstractDataInput OnCreate(IOContext context);

        public void Run(IDictionary<string, string> namedParameters = null)
        {
            var entry = OnCreate(new IOContext(namedParameters));

            entry.Start();
        }


        public void WaitForComplete()
        {
            Task.WaitAll(_compltetions.ToArray());
        }
    }
}
