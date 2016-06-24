using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Lemon.Transform
{
    public abstract class DataFlowPipeline
    {
        private IList<Task> _compltetions;

        protected event Action OnStart;

        protected event Action OnComplete;

        public DataFlowPipeline()
        {
            _compltetions = new List<Task>();
        }

        protected void EnsureComplete(Task completion)
        {
            _compltetions.Add(completion);
        }

        protected abstract AbstractDataInput OnCreate(PipelineContext context);

        public void Run(IDictionary<string, string> namedParameters = null)
        {
            var entry = OnCreate(new PipelineContext(namedParameters));

            if(OnStart != null)
            {
                OnStart();
            }

            entry.Start();
        }


        public void WaitForComplete()
        {
            Task.WaitAll(_compltetions.ToArray());

            if(OnComplete != null)
            {
                OnComplete();
            }
        }
    }
}
