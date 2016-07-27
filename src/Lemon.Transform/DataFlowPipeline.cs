using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Lemon.Transform
{
    public enum PipelineStatus
    {
        New = 0,
        Running = 1,
        Finished = 2,
        Failed = 3
    }

    public abstract class DataFlowPipeline
    {
        private ProgressIndicator _progressIndicator;

        private IList<Task> _compltetions;

        protected event Action OnStart;

        protected event Action OnComplete;

        private PipelineStatus _status;

        public DataFlowPipeline()
        {
            _status = PipelineStatus.New;

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

        public PipelineStatus Run(IDictionary<string, object> namedParameters = null)
        {
            try
            {
                if (_status == PipelineStatus.Running)
                {
                    return _status;
                }

                LogService.Default.Info("data pipeline is running");

                _status = PipelineStatus.Running;

                _progressIndicator.Clear();

                var entry = OnCreate(new PipelineContext(_progressIndicator, namedParameters));

                if (OnStart != null)
                {
                    OnStart();
                }

                entry.Start(namedParameters);

                LogService.Default.Info("wait the pipeline for comptetion");

                Task.WaitAll(_compltetions.ToArray());

                _compltetions.Clear();

                _status = PipelineStatus.Finished;

                if (OnComplete != null)
                {
                    OnComplete();
                }

                LogService.Default.Info("pipeline completed!");
            }
            catch (Exception ex)
            {
                _status = PipelineStatus.Failed;

                LogService.Default.Error(string.Format("pipeline {0} is failed", GetType().Name), ex);
            }

            return _status;
        }

        public Task<PipelineStatus> RunAsync(IDictionary<string, object> namedParameters = null)
        {
            return Task.Run(() =>
            {
                return Run(namedParameters);                
            });
        }

        public IEnumerable<KeyValuePair<string, long>> GetAllProgress()
        {
            return _progressIndicator.GetAllProgress();
        }
    }
}
