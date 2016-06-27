using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Lemon.Transform
{
    public abstract class DataFlowPipeline
    {
        private enum Status
        {
            New = 0,
            Running = 1,
            Finished = 2
        }

        private IList<Task> _compltetions;

        protected event Action OnStart;

        protected event Action OnComplete;

        private Status _status;

        public DataFlowPipeline()
        {
            _status = Status.New;

            _compltetions = new List<Task>();
        }

        protected void EnsureComplete(Task completion)
        {
            _compltetions.Add(completion);
        }

        protected abstract AbstractDataInput OnCreate(PipelineContext context);

        public void Run(IDictionary<string, string> namedParameters = null)
        {
            if(_status == Status.Running)
            {
                return;
            }

            _status = Status.Running;

            var entry = OnCreate(new PipelineContext(namedParameters));

            if(OnStart != null)
            {
                OnStart();
            }

            entry.Start();

            Task.WaitAll(_compltetions.ToArray());

            _status = Status.Finished;
        }

        public Task RunAsync(IDictionary<string, string> namedParameters = null)
        {
            return Task.Run(() =>
            {
                Run(namedParameters);
            });
        }
    }
}
