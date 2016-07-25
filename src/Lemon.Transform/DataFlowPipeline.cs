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

        private ProgressIndicator _progressIndicator;

        private IList<Task> _compltetions;

        protected event Action OnStart;

        protected event Action OnComplete;

        private Status _status;

        public DataFlowPipeline()
        {
            _status = Status.New;

            _compltetions = new List<Task>();

            _progressIndicator = new ProgressIndicator();
        }

        /// <summary>
        /// waits the objects for completes
        /// </summary>
        /// <param name="objects"></param>
        public void Waits(params LinkObject [] objects)
        {
            foreach(var obj in objects)
            {
                _compltetions.Add(obj.Compltetion);
            } 
        }

        protected abstract AbstractDataInput OnCreate(PipelineContext context);

        public void Run(IDictionary<string, string> namedParameters = null)
        {
            try
            {
                if (_status == Status.Running)
                {
                    return;
                }

                LogService.Default.Info("data pipeline is running");

                _status = Status.Running;

                _progressIndicator.Clear();

                var entry = OnCreate(new PipelineContext(_progressIndicator, namedParameters));

                if (OnStart != null)
                {
                    OnStart();
                }

                entry.Start();

                LogService.Default.Info("wait the pipeline for comptetion");

                Task.WaitAll(_compltetions.ToArray());

                _compltetions.Clear();

                _status = Status.Finished;

                if (OnComplete != null)
                {
                    OnComplete();
                }

                LogService.Default.Info("pipeline completed!");
            }
            catch (Exception ex)
            {
                LogService.Default.Error(string.Format("pipeline {0} is failed", GetType().Name), ex);
            }
        }

        public Task RunAsync(IDictionary<string, string> namedParameters = null)
        {
            return Task.Run(() =>
            {
                Run(namedParameters);                
            });
        }

        public IEnumerable<KeyValuePair<string, long>> GetAllProgress()
        {
            return _progressIndicator.GetAllProgress();
        }
    }
}
